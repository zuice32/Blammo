using System;
using System.Data.SQLite;
using System.IO;
using Agent.Core.Logging;

namespace Agent.Core.Logging.Sqlite
{
    public abstract class SqliteRepository
    {
        private readonly object _syncLock = new object();
        private readonly string _pathToDbFile;

        protected SqliteRepository(string pathToDbFile, ILogWriter applicationLog)
        {
            _pathToDbFile = pathToDbFile;

            this.ApplicationLog = applicationLog;
        }

        protected ILogWriter ApplicationLog { get; private set; }

        protected bool IsInitialized { get; private set; }

        protected SQLiteConnection GetConnection()
        {
            return GetConnection(_pathToDbFile);
        }

        protected abstract string ClassName { get; }

        protected virtual SQLiteConnection GetConnection(string pathToDbFile)
        {
            string connectionString = string.Format(
                "Data Source={0};Version=3;PRAGMA integrity_check=10;foreign keys=true;",
                pathToDbFile);

            return new SQLiteConnection(connectionString);
        }

        protected virtual void InitializeBase()
        {
            lock (_syncLock)
            {
                if (this.IsInitialized) return;

                this.IsInitialized = true;

                if (!File.Exists(_pathToDbFile))
                {
                    CreateNewDb(_pathToDbFile);

                    this.ApplicationLog.AddEntry(
                        new LogEntry(
                            MessageLevel.AppLifecycle,
                            ClassName,
                            "New database file created at " + _pathToDbFile));
                }

                VerifyDb(_pathToDbFile, this.ApplicationLog);
            }
        }

        protected void VerifyTable(string tableName, string createTableSql)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();

                bool tableExists;

                string sql = string.Format("SELECT count(name) FROM sqlite_master WHERE name = '{0}'", tableName);

                using (SQLiteCommand testCommand = new SQLiteCommand(sql, dbConnection))
                {
                    tableExists = (long)testCommand.ExecuteScalar() > 0;
                }

                if (!tableExists)
                {
                    using (SQLiteCommand createTable = new SQLiteCommand(createTableSql, dbConnection))
                    {
                        createTable.ExecuteNonQuery();
                    }
                }
            }
        }

        protected void VerifyIndex(string tableName, string createIndexTime)
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();

                bool indexExists;

                string sql = string.Format("SELECT count(name) FROM sqlite_master WHERE name = 'TimeIndex'");

                using (SQLiteCommand testCommand = new SQLiteCommand(sql, dbConnection))
                {
                    indexExists = (long)testCommand.ExecuteScalar() > 0;
                }

                if (!indexExists)
                {
                    using (SQLiteCommand createIndex = new SQLiteCommand(createIndexTime, dbConnection))
                    {
                        createIndex.ExecuteNonQuery();
                    }
                }
            }
        }

        protected void VerifyColumn(string tableName, string columnName, string createColumn)
        {
            bool columnExists = false;

            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                var cmd = dbConnection.CreateCommand();
                cmd.CommandText = string.Format("PRAGMA table_info({0})", tableName);

                var reader = cmd.ExecuteReader();
                int nameIndex = reader.GetOrdinal("Name");
                while (reader.Read())
                {
                    if (reader.GetString(nameIndex).Equals(columnName))
                    {
                        columnExists = true;
                    }
                }


                if (!columnExists)
                {
                    using (SQLiteCommand create = new SQLiteCommand(createColumn, dbConnection))
                    {
                        create.ExecuteNonQuery();
                    }
                }

                dbConnection.Close();
            }
        }

        protected void CheckInitialized()
        {
            lock (_syncLock)
            {
                if (!this.IsInitialized)
                {
                    throw new InvalidOperationException(
                        "'Initialize' method must be called before using an instance of this class.");
                }
            }
        }

        protected virtual void CreateNewDb(string pathToDbFile)
        {
            SQLiteConnection.CreateFile(pathToDbFile);
        }

        protected virtual void VerifyDb(string pathToDbFile, ILogWriter applicationLog)
        {
            try
            {
                using (SQLiteConnection testConnection = this.GetConnection(pathToDbFile))
                {
                    testConnection.Open();

                    using (SQLiteCommand testCommand =
                        new SQLiteCommand("select count(name) from sqlite_master where type = 'table'", testConnection))
                    {
                        testCommand.ExecuteScalar();
                    }
                }
            }
            catch (SQLiteException e)
            {
                if (e.ResultCode == SQLiteErrorCode.NotADb) //not a valid sqlite file
                {
                    this.ReplaceDb(pathToDbFile, applicationLog, e);
                }
                else
                {
                    throw;
                }
            }
        }

        private void ReplaceDb(string pathToDbFile, ILogWriter applicationLog, Exception exception)
        {
            string backupFileName = string.Format(
                "{0}_{1}.invalid",
                pathToDbFile,
                DateTime.UtcNow.ToString("yyyy-MM-ddThh-mm-ss"));

            File.Copy(pathToDbFile, backupFileName);

            File.Delete(pathToDbFile);

            this.CreateNewDb(pathToDbFile);

            applicationLog.AddEntry(
                new LogEntry(
                    MessageLevel.Warning,
                    ClassName,
                    string.Format(
                        "Invalid database file found at {0}. Replacement file created.{1}Error:{2}",
                        pathToDbFile,
                        Environment.NewLine,
                        exception.Message)));
        }

    }
}