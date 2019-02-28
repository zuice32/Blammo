using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostReading
    {
        Guid HostId { get; }

        DateTime Time { get; set; }
    }

    public interface IAggregateSensorHostReading : ISensorHostReading
    {
        double AggregationPeriodMinutes { set; }
    }
}
