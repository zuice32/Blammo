using System;
using System.Collections.Generic;

namespace Agent.Core.Exceptions
{
    public class GeneralExceptionFloodStrategy : IExceptionStrategy
    {
        private readonly TimeSpan _defaultFloodPeriod;
        private readonly TimeSpan _defaultReportPeriod;

        private readonly Dictionary<Type, ExceptionFloodWatcher> _floodWatchers =
            new Dictionary<Type, ExceptionFloodWatcher>();

        private readonly object _syncLock = new object();

        public GeneralExceptionFloodStrategy(TimeSpan defaultFloodPeriod, TimeSpan defaultReportPeriod)
        {
            _defaultFloodPeriod = defaultFloodPeriod;
            _defaultReportPeriod = defaultReportPeriod;
        }

        public Dictionary<Type, ExceptionFloodWatcher> FloodWatchers
        {
            get { return _floodWatchers; }
        }

        public Exception ProcessException(Exception e)
        {
            lock (_syncLock)
            {
                if (e == null) return null;

                Type exceptionType = e.GetType();

                //don't throttle flood exceptions
                if (exceptionType == typeof(ExceptionFloodException)) return e;

                if (!FloodWatchers.ContainsKey(exceptionType))
                {
                    FloodWatchers.Add(exceptionType,
                                      new ExceptionFloodWatcher(_defaultFloodPeriod, _defaultReportPeriod));
                }

                ExceptionFloodWatcher floodWatcher = FloodWatchers[exceptionType];

                return floodWatcher.ProcessException(e);
            }
        }
    }
}