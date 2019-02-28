using System;
using System.Collections.Generic;

namespace Agent.Core.Settings
{
    /// <summary>
    /// Placeholder that allows default logging filter(s) to work
    /// in application that don't otherwise use settings.
    /// Doesn't actually write anything to disk.
    /// </summary>
    public class InMemorySettingsProvider : ISettingsProvider
    {
        private readonly Dictionary<SettingKey, string> _settings = new Dictionary<SettingKey, string>();

        public string GetSetting(string owner, string setting, string defaultValue)
        {
            SettingKey key = new SettingKey(owner, setting);

            if (this._settings.ContainsKey(key))
            {
                return this._settings[key];
            }
            
            this._settings.Add(key, defaultValue);

            return defaultValue;
        }

        public void UpsertSetting(string owner, string setting, string value)
        {
            SettingKey key = new SettingKey(owner, setting);

            if (this._settings.ContainsKey(key))
            {
                this._settings[key] = value;
            }
            else
            {
                this._settings.Add(key, value);
            }

            if (this.SettingsChanged != null)
            {
                this.SettingsChanged();
            }
        }

        public Setting[] GetAllSettings()
        {
            throw new NotImplementedException();
        }

//        public void Refresh()
//        {
//            if (this.SettingsChanged != null)
//            {
//                this.SettingsChanged();
//            }
//        }

        public event Action SettingsChanged;

        private class SettingKey : IEquatable<SettingKey>
        {
            public SettingKey(string owner, string setting)
            {
                this.Owner = owner;
                this.Setting = setting;
            }

            public bool Equals(SettingKey other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(this.Owner, other.Owner) && string.Equals(this.Setting, other.Setting);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((SettingKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.Owner.GetHashCode()*397) ^ this.Setting.GetHashCode();
                }
            }

            public static bool operator ==(SettingKey left, SettingKey right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(SettingKey left, SettingKey right)
            {
                return !Equals(left, right);
            }

// ReSharper disable UnusedAutoPropertyAccessor.Local
            private string Owner{ get; set;}

            private string Setting{ get; set;}
// ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}