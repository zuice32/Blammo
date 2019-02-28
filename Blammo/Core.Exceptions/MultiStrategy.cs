using System;
using System.Collections.Generic;

namespace Agent.Core.Exceptions
{
    /// <summary>
    /// Excecutes multiple strategies as a unit.
    /// Strategies that throw exceptions are automatically removed from the strategy list.
    /// </summary>
    public class MultiStrategy : IMultiStrategy
    {
        private readonly List<IExceptionStrategy> _strategies = new List<IExceptionStrategy>();

        public Exception ProcessException(Exception e)
        {
            return this.ExecuteStrategies(e);
        }

        private Exception ExecuteStrategies(Exception e)
        {
            IEnumerable<IExceptionStrategy> strategies = _strategies.ToArray();

            foreach (IExceptionStrategy strategy in strategies)
            {
                try
                {
                    e = strategy.ProcessException(e);
                }
                catch (Exception strategyExeception)
                {
                    this.Strategies.Remove(strategy);

                    ExceptionHandler.HandleException(strategyExeception);
                }
            }

            return e;
        }

        public IList<IExceptionStrategy> Strategies { get { return _strategies; } }
    }
}