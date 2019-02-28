
namespace Agent.Core.Logging
{
    public interface ILogWriter
    {
        void AddEntry(ILogEntry logEntry);
    }
}