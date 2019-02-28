using System.Collections.Generic;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostReadingRecorder
    {
        void UpsertReadings<TReading>(IEnumerable<TReading> readings) where TReading : ISensorHostReading;
        void RecordReadings<TReading>(IEnumerable<TReading> readings) where TReading: ISensorHostReading;
    }
}