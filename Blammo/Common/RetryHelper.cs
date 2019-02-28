using System;
using System.Threading;

namespace Agent.Common
{
    public static class RetryHelper
    {
        public static bool RetryOnFailure(Func<bool> mainAction, int maxRetries, TimeSpan intervalBetweenRetries)
        {
            while ( maxRetries-- > 0) 
            {
                if (mainAction())
                {
                    return true;
                }

                if (intervalBetweenRetries > TimeSpan.Zero)
                {
                    Thread.Sleep(intervalBetweenRetries.Milliseconds);
                }

            } 

            return false;
        }

        public static void RetryOnException(Action mainAction,
                                    int maxRetries,
                                    int intervalBetweenRetriesMs)
        {
            while ( maxRetries-- > 0) 
            {
                try
                {
                    mainAction();
                    break;
                }
                catch (Exception e)
                {
                    if (maxRetries == 0)
                    {
                        throw;
                    }

                    if (intervalBetweenRetriesMs > 0)
                    {
                        Thread.Sleep(intervalBetweenRetriesMs);
                    }
                }
            } 
        }

        //Adapted from MS Extreme Computing Group http://azurescope.cloudapp.net/CodeSamples/cs/095d4436-c9fb-42e8-8164-ec2383e0189d/
        public static void RetryOnException<T>(Action mainAction,
                                               Func<T, bool> exceptionAction,
                                               int maxRetries,
                                               TimeSpan intervalBetweenRetries,
                                               ref int retryCounter) where T : Exception
        {
            while ( maxRetries-- > 0) 
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
            } 
        }

        public static TResult RetryOnException<TArg, TResult, TException>(Func<TArg, TResult> mainAction,
                                                                          Func<TException, bool> exceptionAction,
                                                                          int maxRetries,
                                                                          TimeSpan intervalBetweenRetries,
                                                                          ref int retryCounter,
                                                                          TArg input) where TException : Exception
        {
            TResult result = default(TResult);

            while ( maxRetries-- > 0) 
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
            } 

            return result;
        }
    }
}