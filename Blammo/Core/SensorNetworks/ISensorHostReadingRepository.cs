using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostReadingRepository : ISensorHostReadingRecorder
    {
        TReading[] GetReadings<TReading>(Guid sensorHostId, DateTime startTime, DateTime endTime)
            where TReading : ISensorHostReading;

        TReading[] GetReadings<TReading>(Guid sensorHostId) where TReading : ISensorHostReading;

        string[] GetFullRange(Guid sensorHostId);
    }
}