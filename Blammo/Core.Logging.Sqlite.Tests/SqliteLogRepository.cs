using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Agent.Core.Application;
using Agent.Core.Logging;
using Agent.Core.Logging.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Logging.Sqlite.Tests
{
    [TestClass]
    public class SqliteLogRepositoryTests
    {
        private static readonly string _pathToDbDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            string[] testFiles = Directory.GetFiles(_pathToDbDirectory, "*.log").ToArray();

            Array.ForEach(testFiles, File.Delete);
        }

        [TestMethod]
        public void Initialize_adds_log_table_to_db()
        {
            IAgentIdentity agentIdentity = new AgentIdentity
            {
                ID = "agentId" + DateTime.UtcNow.Ticks,
                PathToAgentDataDirectory = _pathToDbDirectory
            };

            SqliteLogRepositoryDecorator logRepository = new SqliteLogRepositoryDecorator(
                agentIdentity,
                new InMemoryLogWriter(100));

            logRepository.Initialize();

            using (SQLiteConnection dbConnection = logRepository.GetNewDbConnection())
            {
                dbConnection.Open();

                bool logTableExists;

                string sql = "SELECT count(name) FROM sqlite_master WHERE name = 'log'";

                using (SQLiteCommand testCommand = new SQLiteCommand(sql, dbConnection))
                {
                    logTableExists = (long) testCommand.ExecuteScalar() > 0;
                }

                Assert.IsTrue(logTableExists, "Log table not created.");

                sql = "select * from log limit 1";

                using (SQLiteCommand testCommand = new SQLiteCommand(sql, dbConnection))
                {
                    using (SQLiteDataReader dataReader = testCommand.ExecuteReader())
                    {
                        string[] columnNames =
                            Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToArray();

                        Assert.IsTrue(columnNames.Contains("ID"), "ID column missing from Log table.");
                        Assert.IsTrue(columnNames.Contains("Time"), "Time column missing from Log table.");
                        Assert.IsTrue(columnNames.Contains("Level"), "Level column missing from Log table.");
                        Assert.IsTrue(columnNames.Contains("Source"), "Source column missing from Log table.");
                        Assert.IsTrue(columnNames.Contains("Message"), "Message column missing from Log table.");
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Add_throws_exception_if_called_before_Initialize()
        {
            IAgentIdentity agentIdentity = new AgentIdentity
            {
                ID = "agentId" + DateTime.UtcNow.Ticks,
                PathToAgentDataDirectory = _pathToDbDirectory
            };

            SqliteLogRepository logRepository = new SqliteLogRepository(agentIdentity, new InMemoryLogWriter(100));

            logRepository.Add(new LogEntry(MessageLevel.Verbose, "add test", "should throw exception"));
        }

        [TestMethod]
        public void Add_inserts_new_record_in_log_table()
        {
            IAgentIdentity agentIdentity = new AgentIdentity
            {
                ID = "agentId" + DateTime.UtcNow.Ticks,
                PathToAgentDataDirectory = _pathToDbDirectory
            };

            SqliteLogRepositoryDecorator logRepository = new SqliteLogRepositoryDecorator(
                agentIdentity,
                new InMemoryLogWriter(100));

            logRepository.Initialize();

            logRepository.Add(new LogEntry(MessageLevel.Verbose, "add test", "should add this row to table"));

            using (SQLiteConnection dbConnection = logRepository.GetNewDbConnection())
            {
                dbConnection.Open();

                string sql = "SELECT * FROM Log";

                using (SQLiteCommand dbCommand = new SQLiteCommand(sql, dbConnection))
                {
                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        Assert.IsTrue(dataReader.HasRows, "Row not added to Log table.");

                        dataReader.Read();

                        object[] columnValues =
                            Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetValue).ToArray();

                        Assert.IsTrue(columnValues[1] is DateTime, "'Time' column value incorrect.");
                        Assert.AreEqual(
                            (int) MessageLevel.Verbose,
                            Convert.ToInt32(columnValues[2]),
                            "'Level' column value incorrect.");
                        Assert.AreEqual("add test", columnValues[3], "'Source' column value incorrect.");
                        Assert.AreEqual(
                            "should add this row to table",
                            columnValues[4],
                            "'Message' column value incorrect.");
                    }
                }
            }
        }

        [TestMethod]
        public void Add_enforces_max_table_size()
        {
            int maxEntryCount = 100;

            IAgentIdentity agentIdentity = new AgentIdentity
            {
                ID = "agentId" + DateTime.UtcNow.Ticks,
                PathToAgentDataDirectory = _pathToDbDirectory
            };

            SqliteLogRepository logRepository = new SqliteLogRepositoryDecorator(
                agentIdentity,
                new InMemoryLogWriter(100),
                maxEntryCount);

            logRepository.Initialize();

            //default Add operations per trim execution is 100
            for (int entryNumber = 0; entryNumber < maxEntryCount + 1; entryNumber++)
            {
                logRepository.Add(new LogEntry(MessageLevel.Verbose, "add test", entryNumber.ToString()));

                Thread.Sleep(1);
            }

            using (SQLiteConnection dbConnection = ((SqliteLogRepositoryDecorator)logRepository).GetNewDbConnection())
            {
                dbConnection.Open();

                using (SQLiteCommand dbCommand = new SQLiteCommand("SELECT count(*) FROM Log", dbConnection))
                {
                    long rowCount = (long) dbCommand.ExecuteScalar();

                    Assert.AreEqual(maxEntryCount, rowCount, "Incorrect number of rows trimmed from Log table.");
                }

                using (
                    SQLiteCommand dbCommand = new SQLiteCommand(
                        "SELECT Message FROM Log ORDER BY Time LIMIT 1",
                        dbConnection))
                {
                    string oldestMessage = (string) dbCommand.ExecuteScalar();

                    Assert.AreEqual("1", oldestMessage, "Incorrect rows trimmed from table.");
                }

                using (
                    SQLiteCommand dbCommand = new SQLiteCommand(
                        "SELECT Message FROM Log ORDER BY Time DESC LIMIT 1",
                        dbConnection))
                {
                    string newestMessage = (string) dbCommand.ExecuteScalar();

                    Assert.AreEqual("100", newestMessage, "Incorrect rows trimmed from table.");
                }
            }
        }
        
        [TestMethod]
        public void All_locks_on_db_file_release_after_being_accessed()
        {
            IAgentIdentity agentIdentity = new AgentIdentity
            {
                ID = DateTime.UtcNow.Ticks.ToString(),
                PathToAgentDataDirectory = _pathToDbDirectory
            };

            SqliteLogRepository logRepository = new SqliteLogRepository(
                agentIdentity,
                new InMemoryLogWriter(100));

            logRepository.Initialize();
            
            string[] testFiles = Directory.GetFiles(_pathToDbDirectory, "*.log").ToArray();
            
            Array.ForEach(testFiles, File.Delete);
        }
    }
}