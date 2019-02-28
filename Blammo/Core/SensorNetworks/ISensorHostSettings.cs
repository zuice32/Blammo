using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostSettings
    {
        Guid HostId { get; }

        DateTime Time { get; set; }
    }
}
