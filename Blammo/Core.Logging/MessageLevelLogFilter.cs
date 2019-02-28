using System;
using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    /// <summary>
    ///     Filters log entries based on their MessageLevel property.
    /// </summary>
    public class MessageLevelLogFilter : ILogFilter
    {
        private const string _maximummessagelevel = "MaximumMessageLevel";

        private readonly ISettingsProvider _settingsProvider;

        public MessageLevelLogFilter(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null) throw new ArgumentNullException("settingsProvider");
            this._settingsProvider = settingsProvider;
        }

        public string ThisClassName
        {
            get { return "Agent.Core.Logging.MessageLevelLogFilter"; }
        }

        /// <summary>
        ///     Default is true.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return
                    Convert.ToBoolean(this._settingsProvider.GetSetting(this.ThisClassName, "IsEnabled", true.ToString()));
            }
            set { this._settingsProvider.UpsertSetting(this.ThisClassName, "IsEnabled", value.ToString()); }
        }

        public MessageLevel MaximiumMessageLevel
        {
            get
            {
                MessageLevel defaultLevel = MessageLevel.Verbose; 

#if DEBUG
                defaultLevel = MessageLevel.Verbose;
#endif

                string levelString = this._settingsProvider.GetSetting(this.ThisClassName, _maximummessagelevel, defaultLevel.ToString());

                return (MessageLevel) Enum.Parse(typeof (MessageLevel), levelString, true);
            }
            set
            {
                this._settingsProvider.UpsertSetting(this.ThisClassName, _maximummessagelevel, value.ToString());
            }
        }

        /// <summary>
        ///     Returns null if entry does not pass filter criteria.
        /// </summary>
        /// <param name="inputLogEntry"></param>
        /// <returns></returns>
        public ILogEntry Filter(ILogEntry inputLogEntry)
        {
            if (inputLogEntry != null)
            {
                if (this.IsEnabled && inputLogEntry.Level > this.MaximiumMessageLevel)
                {
                    inputLogEntry = null;
                }
            }

            return inputLogEntry;
        }
    }
}