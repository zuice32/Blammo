using System;
using Agent.Common.Collections;

namespace Agent.Core.Logging
{
    public class InMemoryLogWriter : ILogWriter
    {
        public readonly CircularList<ILogEntry> Entries;

        public InMemoryLogWriter(uint maxEntries)
        {
            Entries = new CircularList<ILogEntry>(maxEntries);
        }

        public void AddEntry(ILogEntry logEntry)
        {
            Console.WriteLine("{0} {1} {2}", DateTime.Now, logEntry.Source, logEntry.Message);
            Entries.Add(logEntry);
        }
    }
}