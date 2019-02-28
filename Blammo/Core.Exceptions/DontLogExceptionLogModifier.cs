using System;
using Agent.Core.Logging;

namespace Agent.Core.Exceptions
{
    public class DontLogExceptionLogModifier<T> : ExceptionLogEntryModifierBase<T>
        where T : Exception
    {
        #region Overrides of ExceptionLogEntryModifierBase<T>

        protected override ILogEntry ModifyEntry(T exception, ILogEntry entry)
        {
            return null;
        }

        #endregion
    }
}