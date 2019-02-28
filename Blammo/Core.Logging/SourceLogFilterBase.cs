using System;
using System.Collections.Generic;
using System.Linq;
using Agent.Core.Settings;

namespace Agent.Core.Logging
{
    public abstract class SourceLogFilterBase : ILogFilter
    {
        private readonly ISettingsProvider _settingsProvider;
        private IList<string> _filterTokens;

        protected SourceLogFilterBase(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null) throw new ArgumentNullException("settingsProvider");
            this._settingsProvider = settingsProvider;
        }
        
        public abstract string ThisClassName { get; }

        protected bool IsWhiteFilter { get; set; }

        protected IList<string> SourceFilterTokens
        {
            get
            {   
                if (this._filterTokens == null)
                {
                    string defaultTokens = this.GetDefaultTokens();

                    string tokens = _settingsProvider.GetSetting(this.ThisClassName,
                            "SourceFilterTokens",
                            defaultTokens);

                    tokens = tokens.Trim();

                    this._filterTokens = tokens != string.Empty
                        ? tokens.Split(new[] {' '}).ToList()
                        : new List<string>();

                    _settingsProvider.SettingsChanged += this.SettingsProviderSettingsChanged;
                }

                return this._filterTokens;
            }
        }

        protected abstract string GetDefaultTokens();

        private void SettingsProviderSettingsChanged()
        {
            //reset filter tokens
            this._filterTokens = null;
        }

        /// <summary>
        ///     Default is false.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return
                    Convert.ToBoolean(_settingsProvider.GetSetting(this.ThisClassName, "IsEnabled", false.ToString()));
            }
            set { _settingsProvider.UpsertSetting(this.ThisClassName, "IsEnabled", value.ToString()); }
        }

        public ILogEntry Filter(ILogEntry inputLogEntry)
        {
            ILogEntry outputLogEntry = inputLogEntry;

            if (this.IsEnabled)
            {
                if (inputLogEntry != null)
                {
                    foreach (string filterToken in this.SourceFilterTokens)
                    {
                        if (filterToken != null)
                        {
                            if (inputLogEntry.Source.Contains(filterToken))
                            {
                                outputLogEntry = this.IsWhiteFilter ? inputLogEntry : null;

                                break;
                            }
                        }
                    }
                }
            }

            return outputLogEntry;
        }


    }
}