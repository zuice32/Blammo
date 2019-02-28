using System;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostFaultRecorder
    {
        void RecordSensorHostFault(string message, Guid hostId);
    }
}