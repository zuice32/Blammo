using System.Data.SQLite;
using Agent.Core.Application;
using Agent.Core.Logging;
using Agent.Core.Logging.Sqlite;

namespace Core.Logging.Sqlite.Tests
{
    internal class SqliteLogRepositoryDecorator : SqliteLogRepository
    {
        public SqliteLogRepositoryDecorator(IAgentIdentity agentIdentity, ILogWriter applicationLog)
            : this(agentIdentity, applicationLog, 1000000)
        {
        }

        public SqliteLogRepositoryDecorator(IAgentIdentity agentIdentity,
            ILogWriter applicationLog,
            int maxEntryCount) : base(agentIdentity, applicationLog, maxEntryCount)
        {
        }

        public SQLiteConnection GetNewDbConnection()
        {
            return base.GetConnection();
        }
    }
}