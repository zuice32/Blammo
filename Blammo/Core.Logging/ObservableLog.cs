using Agent.Common.Collections;

namespace Agent.Core.Logging
{
    public class ObservableLog : CircularBuffer<ILogEntry>, ILogWriter
    {
        public ObservableLog(uint maxEntries) : base(maxEntries)
        {
        }

        public void AddEntry(ILogEntry logEntry)
        {
            Add(logEntry);
        }
    }
}