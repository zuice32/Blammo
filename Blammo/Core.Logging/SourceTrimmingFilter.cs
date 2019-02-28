using System;
using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    /// <summary>
    ///     Trims the Source property to remove namespaces from fully-qualified type names.
    /// </summary>
    public class SourceTrimmingFilter : ILogFilter
    {
        private readonly string ThisClassName = "Agent.Core.Logging.SourceTrimmingFilter";
        private readonly ISettingsProvider _settingsProvider;

        public SourceTrimmingFilter(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null) throw new ArgumentNullException("settingsProvider");
            this._settingsProvider = settingsProvider;
        }

        /// <summary>
        ///     Default is true.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return Convert.ToBoolean(
                    this._settingsProvider.GetSetting(this.ThisClassName, "IsEnabled", true.ToString()));
            }
            set { this._settingsProvider.UpsertSetting(this.ThisClassName, "IsEnabled", value.ToString()); }
        }

        /// <summary>
        ///     Returns null if entry does not pass filter criteria.
        /// </summary>
        /// <param name="inputLogEntry"></param>
        /// <returns></returns>
        public ILogEntry Filter(ILogEntry inputLogEntry)
        {
            if (this.IsEnabled)
            {
                if (inputLogEntry != null)
                {
                    string[] sourceTokens = inputLogEntry.Source.Split('.');

                    inputLogEntry.Source = sourceTokens[sourceTokens.Length - 1];
                }
            }

            return inputLogEntry;
        }
    }
}