using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    /// <summary>
    ///     Filter log entries using a 'white list' of values to compare against the entry source.
    ///     Entries from sources that do not match any values in the 'white list' are not logged.
    /// </summary>
    public class WhiteSourceLogFilter : SourceLogFilterBase
    {
        public WhiteSourceLogFilter(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
            IsWhiteFilter = true;
        }

        public override string ThisClassName
        {
            get { return "Agent.Core.Logging.WhiteSourceLogFilter"; }
        }

        protected override string GetDefaultTokens()
        {
            string topLevelNamespaces = "System Microsoft";

            topLevelNamespaces += " " + "Agent";

            topLevelNamespaces += " " + "(unknown source)";

            return topLevelNamespaces;
        }
    }
}