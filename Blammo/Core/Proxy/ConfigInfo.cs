using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtocolHelper;

namespace Agent.Core.Proxy
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigInfo
    {
        struct ConfigStruct
        {
            [BitfieldLength(1)]
            public uint IsNetSuper;
            [BitfieldLength(1)]
            public uint Boost;
            [BitfieldLength(1)]
            public uint Test;
            [BitfieldLength(1)]
            public uint Modbus;
            [BitfieldLength(1)]
            public uint OffScan;
            [BitfieldLength(1)]
            public uint AuxBat;
            [BitfieldLength(1)]
            public uint DebugLevel;
            [BitfieldLength(3)]
            public uint LPMExitCode;
            [BitfieldLength(1)]
            public uint Mux;
            [BitfieldLength(1)]
            public uint Hz900;
            [BitfieldLength(1)]
            public uint Rev3;
            [BitfieldLength(3)]
            public uint Unused;
        };

        private ConfigStruct _config;

        public ConfigInfo()
        {

        }

        public ConfigInfo(ushort value)
        {
            Value = value;
        }

        [JsonProperty]
        public bool IsNetSuper
        {
            get { return _config.IsNetSuper == 1; }
            set { _config.IsNetSuper = (uint)(value ? 1 : 0); }
        }

        [JsonProperty]
        public bool Boost
        {
            get { return _config.Boost == 1; }
            set { _config.Boost = (uint)(value ? 1 : 0); }
        }

        [JsonProperty]
        public bool Test
        {
            get { return _config.Test == 1; }
            set { _config.Test = (uint)(value ? 1 : 0); }
        }

        [JsonProperty]
        public bool Modbus
        {
            get { return _config.Modbus == 1; }
            set { _config.Modbus = (uint)(value ? 1 : 0); }
        }

        public bool OffScan
        {
            get { return _config.OffScan == 1; }
            set { _config.OffScan = (uint)(value ? 1 : 0); }
        }

        public bool AuxBat
        {
            get { return _config.AuxBat == 1; }
            set { _config.AuxBat = (uint)(value ? 1 : 0); }
        }

        public uint DebugLevel
        {
            get { return _config.DebugLevel; }
            set { _config.DebugLevel = value; }
        }

        [JsonProperty]
        public uint LPMExitCode
        {
            get { return _config.LPMExitCode; }
            set { _config.LPMExitCode = (value & 0x7); }
        }

        public bool Mux
        {
            get { return _config.Mux == 1; }
            set { _config.Mux = (uint)(value ? 1 : 0); }
        }

        public bool Hz900
        {
            get { return _config.Hz900 == 1; }
            set { _config.Hz900 = (uint)(value ? 1 : 0); }
        }

        public bool Rev3
        {
            get { return _config.Rev3 == 1; }
            set { _config.Rev3 = (uint)(value ? 1 : 0); }
        }

        public uint Unused
        {
            get { return _config.Unused; }
            set { _config.LPMExitCode = (value & 0xE0); }
        }
        [JsonProperty]
        public ushort Value
        {
            get { return (ushort)PrimitiveConversion.ToLong(_config); }
            private set
            {
                _config = new ConfigStruct
                {
                    IsNetSuper = (uint)((value & 1) > 0 ? 1 : 0),
                    Boost = (uint)((value & 2) > 0 ? 1 : 0),
                    Test = (uint)((value & 4) > 0 ? 1 : 0),
                    Modbus = (uint)((value & 8) > 0 ? 1 : 0),
                    OffScan = (uint)((value & 0x10) > 0 ? 1 : 0),
                    AuxBat = (uint)((value & 0x20) > 0 ? 1 : 0),
                    DebugLevel = (uint)((value & 0x40) > 0 ? 1 : 0),
                    LPMExitCode = (uint)((value & 0x380) >> 7),
                    Mux = (uint)((value & 0x400) > 0 ? 1 : 0),
                    Hz900 = (uint)((value & 0x800) > 0 ? 1 : 0),
                    Rev3 = (uint)((value & 0x1000) > 0 ? 1 : 0),
                    Unused = (uint)((value & 0x3000) >> 13)
                };
            }
        }

        public override string ToString()
        {
            return PrintAsBinary(Value);
        }

        private string PrintAsBinary(ushort value)
        {
            StringBuilder output = new StringBuilder("|");

            for (int index = 15; index >= 0; index--)
            {
                if (index == 8)
                {
                    output.Append("|");
                }

                output.Append((value & (0x1 << index)) > 0 ? "1" : "0");
            }
            output.Append("|");
            return output.ToString();
        }
    }
}
