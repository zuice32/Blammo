
namespace Agent.Core.Logging
{
    public class LogEntry : ILogEntry
    {
        public LogEntry(MessageLevel messageLevel, string source, string message)
        {
            this.Level = messageLevel;
            this.Source = source;
            this.Message = message;
        }

        public MessageLevel Level { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }
    }
}