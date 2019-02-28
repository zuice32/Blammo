using System;
using System.Collections.Generic;

namespace Agent.Core.Exceptions
{
    public interface IMultiStrategy : IExceptionStrategy
    {
        IList<IExceptionStrategy> Strategies { get;  }
    }

    //A little trick to make it more convient to add strategies to a strategy list.
    public static class StrategyListExtensions
    {
        public static void Add(this IList<IExceptionStrategy> strategies,
            Func<Exception, Exception> exceptionDelegate)
        {
            strategies.Add(new ExceptionDelegateWrapper(exceptionDelegate));
        }

        public static void Insert(this IList<IExceptionStrategy> strategies, int index,
            Func<Exception, Exception> exceptionDelegate)
        {
            strategies.Insert(index, new ExceptionDelegateWrapper(exceptionDelegate));
        }
    }

    public class ExceptionDelegateWrapper : IExceptionStrategy
    {
        private readonly Func<Exception, Exception> _handler;

        public ExceptionDelegateWrapper(Func<Exception, Exception> handler)
        {
            _handler = handler;
        }

        public Exception ProcessException(Exception e)
        {
            return _handler.Invoke(e);
        }
    }
}