namespace Agent.Core.Logging.Sqlite
{
    public class SqliteLogWriter : ILogWriter
    {
        private readonly ILogRepository _logRepository;

        public SqliteLogWriter(ILogRepository logRepository )
        {
            _logRepository = logRepository;
        }

        public void AddEntry(ILogEntry logEntry)
        {
            _logRepository.Add(logEntry);
        }
    }
}
