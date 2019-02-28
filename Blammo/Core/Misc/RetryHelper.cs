using System;
using System.Diagnostics;
using System.Threading;

namespace Agent.Core.Misc
{
    public static class RetryHelper
    {
        public static bool RetryOnFailure(Func<bool> mainAction, 
            int maxRetries, TimeSpan intervalBetweenRetries)
        {
            do
            {
                if (mainAction())
                {
                    return true;
                }

                if (intervalBetweenRetries > TimeSpan.Zero)
                {
                    Thread.Sleep(intervalBetweenRetries.Milliseconds);
                }

            } while (--maxRetries > 0);

            return false;
        }

        public static void RetryOnException(Action mainAction, 
                                            int maxRetries, 
                                            int intervalBetweenRetries_mS)
        {
            do
            {
                try
                {
                    mainAction();
                    break;
                }
                catch (Exception e)
                {
                    if (--maxRetries == 0)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    if (intervalBetweenRetries_mS > 0)
                    {
                        Thread.Sleep(intervalBetweenRetries_mS);
                    }
                }
            } while (maxRetries > 0);    
        }

        public static void RetryOnException(Func<bool> mainAction, 
                                        int maxRetries,
                                        TimeSpan intervalBetweenRetries)
        {
            do
            {
                try
                {
                    // Execute the main action.
                    mainAction();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);

                    if (--maxRetries == 0)
                    {
                        throw;
                    }

                    if (intervalBetweenRetries > TimeSpan.Zero)
                    {
                        Thread.Sleep(intervalBetweenRetries.Milliseconds);
                    }
                }
            } while ( maxRetries > 0);
        }

        //Adapted from MS Extreme Computing Group http://azurescope.cloudapp.net/CodeSamples/cs/095d4436-c9fb-42e8-8164-ec2383e0189d/
        public static void RetryOnException<T>(Action mainAction,
                                               Func<T, bool> exceptionAction,
                                               int maxRetries,
                                               int intervalBetweenRetries) where T : Exception
        {
            do
            {
                try
                {
                    // Execute the main action.
                    mainAction();
                    break;
                }
                catch (T exception)
                {

                    // If an action to execute on occurance of exception had been provided, then execute it.
                    if (exceptionAction != null)
                    {
                        bool retry = exceptionAction(exception);

                        if (!retry) maxRetries = 0;
                    }

                    if (--maxRetries == 0)
                    {
                        throw;
                    }
                }
            } while (maxRetries > 0);
        }

        //Adapted from MS Extreme Computing Group http://azurescope.cloudapp.net/CodeSamples/cs/095d4436-c9fb-42e8-8164-ec2383e0189d/
        public static void RetryOnException<T>(Action mainAction,
                                               Func<T, bool> exceptionAction,
                                               int maxRetries,
                                               TimeSpan intervalBetweenRetries,
                                               ref int retryCounter) where T : Exception
        {
            do
            {
                try
                {
                    // Execute the main action.
                    mainAction();
                    break;
                }
                catch (T exception)
                {
                    if (maxRetries == 0)
                    {
                        throw;
                    }
                    retryCounter++;
                    if (intervalBetweenRetries > TimeSpan.Zero)
                    {
                        Thread.Sleep(intervalBetweenRetries.Milliseconds);
                    }
                    // If an action to execute on occurance of exception had been provided, then execute it.
                    if (exceptionAction != null)
                    {
                        bool retry = exceptionAction(exception);

                        if (!retry) maxRetries = 0;
                    }
                }
            } while (maxRetries-- > 0);
        }

        public static TResult RetryOnException<TArg, TResult, TException>(Func<TArg, TResult> mainAction,
                                                                          Func<TException, bool> exceptionAction,
                                                                          int maxRetries,
                                                                          TimeSpan intervalBetweenRetries,
                                                                          ref int retryCounter,
                                                                          TArg input) where TException : Exception
        {
            TResult result = default(TResult);

            do
            {
                try
                {
                    result = mainAction(input);

                    break;
                }
                catch (TException exception)
                {
                    if (maxRetries == 0)
                    {
                        throw;
                    }

                    retryCounter++;

                    if (intervalBetweenRetries > TimeSpan.Zero)
                    {
                        Thread.Sleep(intervalBetweenRetries.Milliseconds);
                    }

                    if (exceptionAction != null)
                    {
                        bool retry = exceptionAction(exception);

                        if (!retry) maxRetries = 0;
                    }
                }
            } while (maxRetries-- > 0);

            return result;
        }
    }
}