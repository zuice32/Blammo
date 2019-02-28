using System;

namespace Agent.Core.Exceptions
{
    public class ExceptionFloodException : Exception
    {
        public ExceptionFloodException(Exception floodingException, int exceptionCount, TimeSpan floodPeriod)
            : base(string.Format("An exception of type {0} has occurred {1} times in the last {2} minutes",
                                 floodingException.GetType().Name,
                                 exceptionCount,
                                 floodPeriod.TotalMinutes),
                   floodingException)
        {
        }
    }
}