using System.Collections.Generic;

namespace Agent.Core.Logging
{
    public interface IApplicationLog : ILogWriter
    {
        List<ILogWriter> LogWriters { get; }

        ILogFilterList LogFilters { get; }
    }
}