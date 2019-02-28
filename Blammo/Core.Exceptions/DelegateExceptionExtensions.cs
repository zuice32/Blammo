using System;
using System.Threading;

namespace Agent.Core.Exceptions
{
    public static class DelegateExceptionExtensions
    {
        /// <summary>
        /// Any exceptions thrown by individual subscribers are 
        /// caught and passed to the input exceptionHandler delegate.
        /// </summary>
        public static void PublishWithExceptionStrategy(this MulticastDelegate eventDelegate,
                                                        IExceptionStrategy exceptionHandler)
        {
            eventDelegate.PublishWithExceptionStrategy(new object[] {}, exceptionHandler);
        }

        /// <summary>
        /// Any exceptions thrown by individual subscribers are 
        /// caught and passed to the input exceptionHandler delegate.
        /// </summary>
        public static void PublishWithExceptionStrategy(this MulticastDelegate eventDelegate,
                                                        object[] parameters,
                                                        IExceptionStrategy exceptionHandler)
        {
            if (eventDelegate != null)
            {
                foreach (Delegate subscriber in eventDelegate.GetInvocationList())
                {
                    try
                    {
                        //Uses MethodInfo.Invoke method to publish event because
                        //Delegate.DynamicInvoke method is not available in Compact Framework 3.5.
                        //DynamicInvoke does essentially the same thing TODO:(? check IL to confirm.)
                        subscriber.Method.Invoke(subscriber.Target, parameters);
                    }
                    catch (Exception e)
                    {
                        // Method.Invoke() wraps any exceptions thrown within the method in a
                        // "TargetInvocationException". We unwrap it here.
                        if (e.InnerException != null)
                        {
                            exceptionHandler.ProcessException(e.InnerException);
                        }
                        else
                        {
                            exceptionHandler.ProcessException(e);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Event delegates are invoked asynchronously.
        /// Any exceptions thrown by individual subscribers are 
        /// caught and passed to the input exceptionHandler.
        /// </summary>
        public static void PublishWithExceptionStrategyAsync(this MulticastDelegate eventDelegate,
                                                             IExceptionStrategy exceptionHandler)
        {
            PublishWithExceptionStrategyAsync(eventDelegate, new object[] {}, exceptionHandler);
        }

        /// <summary>
        /// Event delegates are invoked asynchronously.
        /// Any exceptions thrown by individual subscribers are 
        /// caught and passed to the input exceptionHandler.
        /// </summary>
        public static void PublishWithExceptionStrategyAsync(this MulticastDelegate eventDelegate,
                                                             object[] parameters,
                                                             IExceptionStrategy exceptionHandler)
        {
            if (eventDelegate != null)
            {
                foreach (Delegate subscriber in eventDelegate.GetInvocationList())
                {
                    Delegate eventHandler = subscriber;

                    //Uses MethodInfo.Invoke method to publish event because
                    //Delegate.DynamicInvoke method is not available in Compact Framework 3.5.

                    ThreadPool.QueueUserWorkItem(
                        state =>
                            {
                                try
                                {
                                    eventHandler.Method.Invoke(eventHandler.Target, parameters);
                                }
                                catch (Exception exception)
                                {
                                    // Method.Invoke() wraps any exceptions thrown within the method in a
                                    // "TargetInvocationException". We unwrap it here.
                                    if (exception.InnerException != null)
                                    {
                                        exceptionHandler.ProcessException(exception.InnerException);
                                    }
                                    else
                                    {
                                        exceptionHandler.ProcessException(exception);
                                    }
                                }
                            });
                }
            }
        }

    }
}