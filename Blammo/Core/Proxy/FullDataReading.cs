using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Core.Proxy;
using Agent.Core.SensorNetworks;
using Newtonsoft.Json;

namespace Agent.Core.Proxy
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FullDataReading : IAggregateSensorHostReading
    {
        [JsonProperty]
        public Guid HostId { get; set; }

        [JsonProperty]
        public uint SerialNumber;

        [JsonProperty]
        public DateTime Time { get; set; }

        public UInt16 Version
        {
            get
            {
                byte msb = this.BaseArray().RawData[16];
                byte lsb = this.BaseArray().RawData[17];
                return (ushort)(msb * 256 + lsb);
            }
        }

        public UInt16 ArrayNumber
        {
            get
            {
                byte msb = this.BaseArray().RawData[18];
                byte lsb = this.BaseArray().RawData[19];
                return (ushort)(msb * 256 + lsb);
            }
        }

        public ConfigInfo ConfigValue
        {
            get
            {
                try
                {
                    byte msb = this.BaseArray().RawData[0];
                    byte lsb = this.BaseArray().RawData[1];
                    return new ConfigInfo((ushort)(msb * 256 + lsb));
                }
                catch (Exception)
                {
                    return new ConfigInfo(0);
                }
            }
        }

        private PartialRawData BaseArray()
        {
            return RawDatas.FirstOrDefault(o => (o.Code == 0xD0) || (o.Code == 0xf8));
        }

        public double AggregationPeriodMinutes { set; private get; }

        public FullDataReading()
        {
            RawDatas = new List<PartialRawData>();
        }

        public FullDataReading(PartialRawData initialRaw)
            : this()
        {
            SerialNumber = initialRaw.SerialNumber;
            Time = initialRaw.SampleTime;
        }

        [JsonProperty]
        public List<PartialRawData> RawDatas;

    }
}
