using System;

namespace Agent.Core.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        static ExceptionHandler()
        {
            DefaultStrategies = new MultiStrategy();
             
//            DefaultStrategies.Strategies.Add(new LoggingStrategy());
        }

        public static IMultiStrategy DefaultStrategies { get; set; }

        /// <param name="strategy">Unhandled exception strategy to be used in addition to default strategies.</param>
        public static void HandleUnhandledAppDomainExceptions(IExceptionStrategy strategy)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, e) => HandleException((Exception) e.ExceptionObject, strategy);
        }

        /// <summary>
        ///     Process the input exception using the default exception strategy stack.
        /// </summary>
        /// <param name="e">Exception to process.</param>
        public static void HandleException(Exception e)
        {
            HandleException(e, new NullStrategy());
        }

        /// <summary>
        ///     Process the input exception using the input IExceptionStrategy first
        ///     and then the default strategy stack.
        /// </summary>
        /// <param name="e">Exception to process.</param>
        /// <param name="strategy">First exception strategy to use in processing the exception.</param>
        public static void HandleException(Exception e, IExceptionStrategy strategy)
        {
            HandleException(e, strategy.ProcessException);
        }

        /// <summary>
        ///     Process the input exception using the input IExceptionStrategy first
        ///     and then the default strategy stack.
        /// </summary>
        /// <param name="e">Exception to process.</param>
        /// <param name="strategy">Delegate that represents the first exception strategy to process the exception.</param>
        public static void HandleException(Exception e, Func<Exception, Exception> strategy)
        {
            try
            {
                e = strategy(e);
            }
            catch (Exception strategyExeception)
            {
                HandleException(strategyExeception);
            }

            DefaultStrategies.ProcessException(e);
        }


        void IExceptionHandler.HandleException(Exception e)
        {
            HandleException(e);
        }

        void IExceptionHandler.HandleException(Exception e, IExceptionStrategy strategy)
        {
            HandleException(e, strategy);
        }

        void IExceptionHandler.HandleException(Exception e, Func<Exception, Exception> strategy)
        {
            HandleException(e, strategy);
        }
    }
}