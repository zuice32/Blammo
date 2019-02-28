using System;

namespace Agent.Core.Exceptions
{
    /// <summary>
    /// Keeps multiple exceptions of the same type from clogging up the intertubes..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExceptionFloodStrategy<T>: IExceptionStrategy where T:Exception
    {
        protected DateTime LastExceptionTime { get; set; }
        protected TimeSpan Window { get; set; }

        public ExceptionFloodStrategy(TimeSpan window)
        {
            Window = window;
            LastExceptionTime = DateTime.Now - Window ; 
        }

        public Exception ProcessException(Exception e)
        {
            if(e is T)
            {
                if((DateTime.Now - LastExceptionTime) < Window)
                {
                    return null;
                }

                LastExceptionTime = DateTime.Now;
            
            }
            
            return e;
        }

        
    }
}
