using System;
using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    public class ConsoleLogWriter : ILogWriter
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly string ThisClassName = "Agent.Core.Logging.ConsoleLogWriter";

        public ConsoleLogWriter(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null) throw new ArgumentNullException("settingsProvider");
            this._settingsProvider = settingsProvider;
        }

        public void AddEntry(ILogEntry logEntry)
        {
            if (this.IsEnabled)
            {
                Console.WriteLine(
                TextLogEntryFormatter.GetEntry(logEntry.Level, logEntry.Source, logEntry.Message));
                
            }
        }
        /// <summary>
        /// Default is false.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
#if DEBUG
                return true;
#endif
#pragma warning disable 162
                return Convert.ToBoolean(_settingsProvider.GetSetting(ThisClassName, "IsEnabled", false.ToString()));
#pragma warning restore 162
            }
            set
            {
                _settingsProvider.UpsertSetting(ThisClassName, "IsEnabled", value.ToString());
            }
        }
    }
}
