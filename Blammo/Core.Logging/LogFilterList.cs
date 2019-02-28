using System;
using System.Collections.Generic;

namespace Agent.Core.Logging
{
    public class LogFilterList : List<ILogFilter>, ILogFilterList 
    {
        private readonly Action<Exception> _exceptionHandler;

        public LogFilterList(Action<Exception> exceptionHandler)
        {
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            this._exceptionHandler = exceptionHandler;
        }

        /// <summary>
        ///     Returns null if entry does not pass filter criteria.
        /// </summary>
        /// <param name="inputLogEntry"></param>
        /// <returns></returns>
        public ILogEntry Filter(ILogEntry inputLogEntry)
        {
            ILogFilter[] filters = ToArray();

            foreach (ILogFilter filter in filters)
            {
                try
                {
                    inputLogEntry = filter.Filter(inputLogEntry);
                }
                catch (Exception e)
                {
                    Remove(filter);

                    this._exceptionHandler(e);
                }

                if (inputLogEntry == null) break;
            }

            return inputLogEntry;
        }
    }
}