namespace Agent.Core.Logging
{
    public interface ILogRepository
    {
        void Add(ILogEntry logEntry);
    }
}