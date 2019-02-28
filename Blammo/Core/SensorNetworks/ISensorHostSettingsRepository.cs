using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostSettingsRepository
    {
        THostSettings[] GetSensorHostStateHistory<THostSettings>(Guid hostId, DateTime startTime, DateTime endTime) where THostSettings : ISensorHostSettings;

        void AddIfChanged<THostSettings>(THostSettings hostSettings) where THostSettings : ISensorHostSettings;

    }
}
