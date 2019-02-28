using System;
using System.Collections.ObjectModel;
using Agent.Core.Services;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorNetworkService<TReading, THostInfo> : ISensorNetworkService
        where THostInfo : ISensorHostInfo
    {
        ReadOnlyObservableCollection<ISensorHostProxy<TReading, THostInfo>> SensorHosts { get; }
    }

    public interface ISensorNetworkService : IApplicationService
    {
        /// <summary>
        /// Performs a network discovery and updates the SensorHosts collection
        /// and the list of sensor hosts in the db to match the current state of 
        /// the network.
        /// </summary>
        void UpdateSensorHostCollection();

        string Description { get; }

        int SensorHostCount { get; }

        bool IsEnabled { get; }

        DateTime LastRefreshTime { get; set; }
    
    }
}