using System;
using System.Collections.ObjectModel;
using Agent.Core.Services;

namespace Agent.Core.Sensor
{
    public interface ISensorService<TReading, TSInfo> : ISensorService
           where TSInfo : ISensorInfo
    {
        ReadOnlyObservableCollection<ISensorProxy<TReading, TSInfo>> Sensors { get; }
    }

    public interface ISensorService : IApplicationService
    {
        /// <summary>
        /// Performs a network discovery and updates the Sensor collection
        /// and the list of sensors in the db to match the current state of 
        /// the sensor host is to the network.
        /// </summary>
        void UpdateSensorCollection();

        string Description { get; }

        int SensorCount { get; }

        DateTime LastRefreshTime { get; set; }

    }
}
