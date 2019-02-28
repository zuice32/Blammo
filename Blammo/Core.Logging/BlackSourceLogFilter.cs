using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    public class BlackSourceLogFilter : SourceLogFilterBase
    {
        private readonly string _defaultTokens = string.Empty;

        public BlackSourceLogFilter(string defaultTokens, ISettingsProvider settingsProvider)
            : this(settingsProvider)
        {
            this._defaultTokens = defaultTokens;
        }

        public BlackSourceLogFilter(ISettingsProvider settingsProvider) : base(settingsProvider)
        {
            base.IsWhiteFilter = false;
        }

        public override string ThisClassName
        {
            get { return "Agent.Core.Logging.BlackSourceLogFilter"; }
        }

        protected override string GetDefaultTokens()
        {
            return this._defaultTokens;
        }
    }
}