using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Agent.Core.Proxy
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DataReadingBase
    {
        [JsonProperty]
        public uint Code;

        public uint SerialNumber;

        public DateTime SampleTime;

        public UInt16 Version;

        public UInt16 ArrayNumber;
    }
}
