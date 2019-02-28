using System;

namespace Agent.Core.Exceptions
{
    public interface IExceptionStrategy
    {
        /// <summary>
        /// Implement some exception handling behavior and return the input exception
        /// or a new exception for other IExceptionStrategy objects to process.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Exception ProcessException(Exception e);
    }
}