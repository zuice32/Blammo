using System;
using System.Collections.Generic;
using System.Linq;
using Agent.Core.Exceptions;
using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    public class ApplicationLog : IApplicationLog
    {
        public static ApplicationLog GetDefault(IExceptionHandler exceptionHandler)
        {
            ApplicationLog log = new ApplicationLog(exceptionHandler.HandleException);
#if !DEBUG
            log.LogWriters.Add(new InMemoryLogWriter(1000));
#endif
            return log;
        }

        public static void InitDefault(IApplicationLog log, ISettingsProvider settingsProvider)
        {
            InMemoryLogWriter tempWriter = log.LogWriters.OfType<InMemoryLogWriter>().FirstOrDefault();

            if (tempWriter != null)
            {
                log.LogWriters.Remove(tempWriter);

                foreach (ILogEntry entry in tempWriter.Entries)
                {
                    log.AddEntry(entry);
                }
            }

            log.LogFilters.Add(new WhiteSourceLogFilter(settingsProvider));

            log.LogFilters.Add(new BlackSourceLogFilter("Recording Polling", settingsProvider));

            log.LogFilters.Add(new MessageLevelLogFilter(settingsProvider));
        }

        private readonly Action<Exception> _exceptionHandler;

        public ApplicationLog(Action<Exception> exceptionHandler)
        {
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            this._exceptionHandler = exceptionHandler;

            this.SetDefaultLogFilters(this._exceptionHandler);

            this.SetDefaultLogWriters();
        }

        public List<ILogWriter> LogWriters { get; private set; }

        public ILogFilterList LogFilters { get; private set; }

        public void AddEntry(ILogEntry entry)
        {
            entry = this.LogFilters.Filter(entry);

            if (entry != null)
            {
                ILogWriter[] loggers = this.LogWriters.ToArray();

                foreach (ILogWriter logger in loggers)
                {
                    try
                    {
                        logger.AddEntry(entry);
                    }
                    catch (Exception e)
                    {
                        //Don't take any shit from log writer
                        //(and avoid infinite loop).
                        this.LogWriters.Remove(logger);

                        if (this._exceptionHandler != null) this._exceptionHandler(e);
                    }
                }
            }
        }

        private void SetDefaultLogFilters(Action<Exception> exceptionHandler)
        {
            this.LogFilters = new LogFilterList(exceptionHandler);
        }

        private void SetDefaultLogWriters()
        {
            this.LogWriters = new List<ILogWriter>{new DebugLogWriter()};
        }

        public void AddEntry(MessageLevel messageLevel, string source, string message)
        {
            this.AddEntry(new LogEntry(messageLevel, source, message));
        }
    }
}