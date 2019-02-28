using System;
using System.Threading;

namespace Agent.Core.Exceptions
{
    public class ExceptionFloodWatcher
    {
        private readonly TimeSpan _floodPeriod;
        private readonly TimeSpan _reportPeriod;
        private DateTime _previousExceptionTime = DateTime.MinValue;
        private Exception _previousException;
        private int _exceptionCount;
        private Timer _reportingTimer;
        private readonly object _syncLock = new object();

        public ExceptionFloodWatcher(TimeSpan floodPeriod, TimeSpan reportPeriod)
        {
            _floodPeriod = floodPeriod;
            _reportPeriod = reportPeriod;
        }

        public Exception ProcessException(Exception exception)
        {
            lock (_syncLock)
            {
                bool isFlooding = DateTime.UtcNow - _previousExceptionTime < _floodPeriod;

                _previousExceptionTime = DateTime.UtcNow;

                _previousException = exception;

                if (!isFlooding && _reportingTimer == null)
                {
                    return exception;
                }

                _exceptionCount++;

                if (_reportingTimer == null)
                {
                    //timer is to only fire once
                    _reportingTimer = new Timer(this.ReportingTimerCallback,
                                                null,
                                                Convert.ToInt32(_reportPeriod.TotalMilliseconds),
                                                Timeout.Infinite);
                }

                return null;
            }
        }

        private void ReportingTimerCallback(object state)
        {
            try
            {
                ExceptionFloodException floodException = new ExceptionFloodException(_previousException,
                                                                                     _exceptionCount,
                                                                                     _floodPeriod);

                ExceptionHandler.HandleException(floodException);

                _exceptionCount = 0;

                if (this._reportingTimer != null)
                {
                    this._reportingTimer.Dispose();

                    this._reportingTimer = null;
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }
    }
}