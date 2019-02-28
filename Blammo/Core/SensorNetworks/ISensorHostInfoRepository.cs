using System;
using System.Collections.Generic;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorHostInfoRepository
    {
        THostInfo[] GetHostInfos<THostInfo>(string hostType, string hostSubType) where THostInfo : ISensorHostInfo; 
//        THostInfo[] GetHostInfos(Func<THostInfo, bool> hostSelector, Func<ISensorHostInfo, THostInfo> hostInfoBuilder); 
        THostInfo GetHostInfo<THostInfo>(Guid hostId) where THostInfo : ISensorHostInfo; 

//        void UpsertSensorHostType(string hostTypeId, string hostTypeName);

//        void UpsertHostInfos(IEnumerable<THostInfo> hostInfos, IEqualityComparer<THostInfo> hostComparer);
        void UpsertHostInfos<THostInfo>(IEnumerable<THostInfo> hostInfos) where THostInfo : ISensorHostInfo;

        void MUpsertHostInfos<THostInfo>(IEnumerable<THostInfo> hostInfos, IEnumerable<THostInfo> originalHostInfos) where THostInfo : ISensorHostInfo;

        void MarkDeleteHostInfo<THostInfo>(THostInfo hostInfo) where THostInfo : ISensorHostInfo;

        void DeleteHostInfo<THostInfo>(THostInfo hostInfo) where THostInfo : ISensorHostInfo;

//        void UpdateHostInfos(IEnumerable<THostInfo> hostInfos);
//        
//        void AddHostInfos(IEnumerable<THostInfo> newHostInfos);

        void UpdateHostInfo<THostInfo>(THostInfo hostInfo) where THostInfo : ISensorHostInfo;

//TODO: move to ISensorHostFaultRepository (IHistoryWriter)
//        void RecordSensorHostFault(string message, Guid sensorHostId);  

// TODO: move to ISensorHostReadingRepository (IHistoryWriter)
//        void RecordReadings<TReading>(IEnumerable<TReading> readings); 

        void UpsertHostInfo<THostInfo>(THostInfo hostInfo) where THostInfo : ISensorHostInfo;
    }
}