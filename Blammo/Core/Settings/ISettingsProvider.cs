
using System;

namespace Agent.Core.Settings
{
    public interface ISettingsProvider
    {
        /// <summary>
        /// Retrieves the value for the specified owner and setting.
        /// If the specified owner-setting key doesn't exist in the settings store, 
        /// a new setting is added using the default value provided.
        /// </summary>
        string GetSetting(string owner, string setting, string defaultValue);

        /// <summary>
        /// Retrieves the value stored for the input owner-setting key.
        /// If the owner-setting key doesn't exist in the settings store, 
        /// a the new key is created.
        /// </summary>
        void UpsertSetting(string owner, string setting, string value);

        /// <summary>
        /// Retrieves an array will all the application settings in the store
        /// </summary>
        Setting[] GetAllSettings();

//        /// <summary>
//        /// Refresh all setting values from settings store.
//        /// </summary>
//        void Refresh();

        event Action SettingsChanged;
    }
}
