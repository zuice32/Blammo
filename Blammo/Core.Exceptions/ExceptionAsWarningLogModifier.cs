using System;
using Agent.Core.Logging;

namespace Agent.Core.Exceptions
{
    public class ExceptionAsWarningLogModifier<T> : ExceptionLogEntryModifierBase<T>
        where T : Exception
    {
        public ExceptionAsWarningLogModifier() : this(MessageLevel.Warning)
        {
        }

        public ExceptionAsWarningLogModifier(MessageLevel messageLevel)
        {
            this.MessageLevel = messageLevel;
        }

        protected MessageLevel MessageLevel { get; private set; }

        #region Overrides of ExceptionLogEntryModifierBase<T>

        protected override ILogEntry ModifyEntry(T exception, ILogEntry entry)
        {
            entry.Level = this.MessageLevel;

            return entry;
        }

        #endregion
    }
}