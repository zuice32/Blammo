using System.Collections.Generic;

namespace Agent.Core.Logging
{
    public interface ILogFilterList : IList<ILogFilter>, ILogFilter
    {
    }
}