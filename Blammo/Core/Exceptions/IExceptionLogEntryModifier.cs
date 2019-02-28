using System;
using Agent.Core.Logging;

namespace Agent.Core.Exceptions
{
    public interface IExceptionLogEntryModifier
    {
        ILogEntry Modify(Exception exception, ILogEntry entry);
    }

    public abstract class ExceptionLogEntryModifierBase<T> : IExceptionLogEntryModifier where T : Exception
    {
        #region Implementation of IExceptionLogEntryModifier

        public ILogEntry Modify(Exception exception, ILogEntry entry)
        {
            if (exception is T)
            {
                return this.ModifyEntry((T) exception, entry);
            }

            return entry;
        }

        #endregion

        protected abstract ILogEntry ModifyEntry(T exception, ILogEntry entry);
    }
}