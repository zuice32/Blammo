using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Core.Sensor
{
    public interface ISensorProxy<out TReading, TSInfo> where TSInfo : ISensorInfo
    {
        TSInfo SensorInfo { get; set; }

        event Action<ISensorProxy<TReading, TSInfo>, TReading> ReadingReceived;

        TReading GetReading(DateTime? readingTime);

        DateTime LastGoodCommunicationTime { get; }

    }
}
