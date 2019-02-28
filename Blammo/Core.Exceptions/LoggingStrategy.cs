using System;
using System.Collections.Generic;
using Agent.Core.Logging;

namespace Agent.Core.Exceptions
{
    public class LoggingStrategy : IExceptionStrategy
    {
        private readonly Action<ILogEntry> _logException;
        public const string UnknownExceptionSource = "(unknown source)";

        public static readonly List<IExceptionLogEntryModifier> LogEntryModifiers =
            new List<IExceptionLogEntryModifier>();

        public LoggingStrategy(Action<ILogEntry> logException)
        {
            if (logException == null) throw new ArgumentNullException("logException");
            this._logException = logException;
        }

        public virtual Exception ProcessException(Exception e)
        {
            LogException(e, MessageLevel.Error);

            return e;
        }

        public void LogException(Exception e, MessageLevel messageLevel)
        {
            if (e != null)
            {
                ILogEntry logEntry = BuildExceptionLogEntry(e, messageLevel);

                _logException(logEntry);
            }
        }

        public static ILogEntry BuildExceptionLogEntry(Exception exception, MessageLevel messageLevel)
        {
//#if (WindowsCE || SILVERLIGHT)
//            //System.Exception.Source property doesn't exist in Compact Framework 3.5 or Ag 2.0 beta 2
//            string source = GetSourceFromStackTrace(exception);
//#else
            string source = exception.Source;
//#endif
            string message = GetErrorMessage(exception);

            if (exception.InnerException != null)
            {
                message = message + Environment.NewLine + GetErrorMessage(exception.InnerException);
            }

            ILogEntry logEntry = new LogEntry(messageLevel,
                source,
                message);

            logEntry = ModifyEntry(logEntry, exception);

            return logEntry;
        }

        private static ILogEntry ModifyEntry(ILogEntry entry, Exception exception)
        {
            foreach (IExceptionLogEntryModifier entryModifier in LogEntryModifiers)
            {
                try
                {
                    entry = entryModifier.Modify(exception, entry);
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                }
            }

            return entry;
        }

        private static string GetErrorMessage(Exception e)
        {
            return e.GetType().Name + ":" + e.Message + Environment.NewLine + e.StackTrace;
        }

        public static string GetSourceFromStackTrace(Exception e)
        {
            string source = UnknownExceptionSource;

            if (e.StackTrace != null)
            {
                string trace = e.StackTrace;

                int start = trace.IndexOf("at", StringComparison.Ordinal) + 3;

                int length = trace.IndexOf("(", StringComparison.Ordinal) - start;

                if (trace.Length > 3 && length > 0)
                {
                    source = trace.Substring(start, length);
                }
            }

            return source;
        }
    }
}