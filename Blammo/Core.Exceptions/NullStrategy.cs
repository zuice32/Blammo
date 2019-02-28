using System;

namespace Agent.Core.Exceptions
{
    public class NullStrategy : IExceptionStrategy
    {
        public Exception ProcessException(Exception e)
        {
            //empty strategy

            return e;
        }
    }
}