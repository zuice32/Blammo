using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Core.Sensor
{
    public interface ISensorInfoRepository
    {
        TSInfo[] GetSensorInfos<TSInfo>(string sensorType, string hostSubType) where TSInfo : ISensorInfo;

        TSInfo GetSensorInfo<TSInfo>(Guid sensorId) where TSInfo : ISensorInfo;

        void UpsertSensorInfos<TSInfo>(IEnumerable<TSInfo> sensorInfos) where TSInfo : ISensorInfo;

        void UpdateSensorInfo<TSInfo>(TSInfo sensorInfo) where TSInfo : ISensorInfo;

    }
}