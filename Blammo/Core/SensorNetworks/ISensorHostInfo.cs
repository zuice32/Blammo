using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostInfo
    {
        Guid HostId { get; set; }

        Guid NetworkId { get; set; }

        DateTime LastSuccessfullPollTime { get; set; }

        string Name { get; set; }

        string SerialNumber { get; }

        string HostType { get; set; }

        string HostSubType { get; set; }

//      string Serialized { get; }
    }

}