using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostProxy<out TReading, THostInfo> where THostInfo : ISensorHostInfo
    {
        THostInfo HostInfo{ get; set;}

        event Action<ISensorHostProxy<TReading, THostInfo>, TReading> ReadingReceived;

        TReading GetReading(DateTime? readingTime);
        
        DateTime LastGoodCommunicationTime { get; }

    }
}