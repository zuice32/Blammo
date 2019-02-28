using System;
using System.Security;
using System.Text;
using Agent.Core.Logging;

namespace Agent.Core.Exceptions
{
    public class SecurityExceptionLogModifier : ExceptionLogEntryModifierBase<SecurityException>
    {
        #region Overrides of ExceptionLogEntryModifierBase<SecurityException>

        protected override ILogEntry ModifyEntry(SecurityException exception, ILogEntry entry)
        {
            StringBuilder securityMessageBuilder = new StringBuilder();

            securityMessageBuilder.AppendLine("Action: " + exception.Action.ToString());

            string failedassemblyinfo = "FailedAssemblyInfo: ";
            try
            {
                failedassemblyinfo += exception.FailedAssemblyInfo.FullName;
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
            securityMessageBuilder.AppendLine(failedassemblyinfo);

            string firstpermissionthatfailed = "FirstPermissionThatFailed: ";
            try
            {
                firstpermissionthatfailed += exception.FirstPermissionThatFailed.ToString();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch 
// ReSharper restore EmptyGeneralCatchClause
            {
            }
            securityMessageBuilder.AppendLine(firstpermissionthatfailed);

            entry.Message = entry.Message + Environment.NewLine + securityMessageBuilder.ToString();

            return entry;
        }

        #endregion
    }
}