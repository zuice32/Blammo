using System;

namespace Agent.Core.Logging
{
    public static class TextLogEntryFormatter
    {
        public static string GetEntry(MessageLevel level, string source, string message)
        {
            return string.Format("{0} - {3} - {1} - {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), source, message, level.ToString());
        }
    }
}