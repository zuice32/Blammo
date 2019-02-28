using System;

namespace Agent.Core.Exceptions
{
    public interface IExceptionHandler
    {
        void HandleException(Exception e);
        void HandleException(Exception e, IExceptionStrategy strategy);
        void HandleException(Exception e, Func<Exception, Exception> strategy);
    }
}
