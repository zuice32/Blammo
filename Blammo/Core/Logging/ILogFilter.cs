
namespace Agent.Core.Logging
{
    public interface ILogFilter
    {
        /// <summary>
        /// Returns null if entry does not pass filter criteria.
        /// </summary>
        /// <param name="inputLogEntry"></param>
        /// <returns></returns>
        ILogEntry Filter(ILogEntry inputLogEntry);
    }
}
