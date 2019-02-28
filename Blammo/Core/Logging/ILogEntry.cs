
namespace Agent.Core.Logging
{
    public enum MessageLevel
    {
        Error = 0,
        Warning = 5,
        AppLifecycle = 6,
        ComponentActivity = 7,
        Performance = 8,
        Detail = 9,
        Verbose = 10
    }

    public interface ILogEntry
    {
        MessageLevel Level { get; set; }

        string Source { get; set; }

        string Message { get; set; }
    }
}