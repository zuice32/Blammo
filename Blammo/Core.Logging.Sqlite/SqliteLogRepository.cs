using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Agent.Core.Application;
using Agent.Sqlite;

namespace Agent.Core.Logging.Sqlite
{
    public class SqliteLogRepository : SqliteRepository, ILogRepository
    {
        private int _addOperationsCount;
        private readonly int _addOperationsPerTableTrim;
        private readonly int _maxEntryCount;

        public SqliteLogRepository(IAgentIdentity agentIdentity, ILogWriter applicationLog)
            : this(agentIdentity, applicationLog, 100000)
        {
        }

        public SqliteLogRepository(
            IAgentIdentity agentIdentity,
            ILogWriter applicationLog,
            int maxEntryCount) : base(GetLogFileName(agentIdentity), applicationLog)
        {
            _maxEntryCount = maxEntryCount;

            _addOperationsPerTableTrim = Math.Max(100, maxEntryCount/100);
        }

        protected override string ClassName
        {
            get { return "Agent.Core.Logging.Sqlite.SqliteLogRepository"; }
        }

        private static string GetLogFileName(IAgentIdentity agentIdentity)
        {
            return Path.Combine(agentIdentity.PathToAgentDataDirectory, "agent_" + agentIdentity.ID + ".log");
        }
        
        private SQLiteCommand BuildTrimTableCommand()
        {
            const string sql =
                "DELETE FROM Log WHERE ID NOT IN (SELECT ID FROM Log ORDER BY Time DESC LIMIT @maxRowCount)";

            SQLiteCommand trimTableCommand = new SQLiteCommand(sql);

            trimTableCommand.Parameters.Add("@maxRowCount", DbType.Int32);

            trimTableCommand.Parameters[0].Value = _maxEntryCount;

            return trimTableCommand;
        }

        private SQLiteCommand BuildInsertCommand()
        {
            const string sql =
                "INSERT INTO Log (Time, Level, Source, Message) VALUES (@time, @level, @source, @message)";

            SQLiteCommand insertCommand = new SQLiteCommand(sql);

            insertCommand.Parameters.Add("@time", DbType.DateTime2);
            insertCommand.Parameters.Add("@level", DbType.Int32);
            insertCommand.Parameters.Add("@source", DbType.String);
            insertCommand.Parameters.Add("@message", DbType.String);

            return insertCommand;
        }

        public void Initialize()
        {
            base.InitializeBase();

            this.VerifyLogTable();
        }

        private void VerifyLogTable()
        {
            using (SQLiteConnection dbConnection = base.GetConnection())
            {
                dbConnection.Open();

                bool logTableExists;

                const string sql = "SELECT count(name) FROM sqlite_master WHERE name = 'log'";

                using (SQLiteCommand testCommand = new SQLiteCommand(sql, dbConnection))
                {
                    logTableExists = (long) testCommand.ExecuteScalar() > 0;
                }

                if (!logTableExists)
                {
                    this.CreateLogTable(dbConnection);
                }
            }
        }

        private void CreateLogTable(SQLiteConnection dbConnection)
        {
            const string sql =
                "CREATE TABLE log (ID INTEGER PRIMARY KEY, Time DATETIME NOT NULL, Level INTEGER NOT NULL, Source TEXT NOT NULL, Message TEXT NOT NULL)";

            using (SQLiteCommand createTable = new SQLiteCommand(sql, dbConnection))
            {
                createTable.ExecuteNonQuery();
            }
        }

        public void Add(ILogEntry logEntry)
        {
            base.CheckInitialized();

            using (SQLiteConnection dbConnection = base.GetConnection())
            {
                dbConnection.Open();

                using (SQLiteCommand insertCommand = this.BuildInsertCommand())
                {
                    insertCommand.Connection = dbConnection;

                    using (SQLiteTransaction transaction = dbConnection.BeginTransaction())
                    {
                        insertCommand.Parameters[0].Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                        insertCommand.Parameters[1].Value = (int) logEntry.Level;
                        insertCommand.Parameters[2].Value = logEntry.Source;
                        insertCommand.Parameters[3].Value = logEntry.Message;

                        try
                        {
                            insertCommand.ExecuteNonQuery();

                            transaction.Commit();
                        }
// ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                            
                        }
                    }
                }
            }

            this.EnforceMaxLogSize();
        }

        private void EnforceMaxLogSize()
        {
            _addOperationsCount++;

            if (_addOperationsCount > _addOperationsPerTableTrim)
            {
                _addOperationsCount = 0;

                using (SQLiteConnection dbConnection = base.GetConnection())
                {
                    dbConnection.Open();

                    using (SQLiteCommand trimTableCommand = this.BuildTrimTableCommand())
                    {
                        trimTableCommand.Connection = dbConnection;

                        using (SQLiteTransaction dbTransaction = dbConnection.BeginTransaction())
                        {
                            int rowsTrimmed = trimTableCommand.ExecuteNonQuery();

                            dbTransaction.Commit();

                            base.ApplicationLog.AddEntry(
                                new LogEntry(
                                    MessageLevel.Verbose,
                                    ClassName,
                                    rowsTrimmed + " rows trimmed from Log table."));
                        }
                    }
                }
            }
        }
    }
}