using System;
using Agent.Core.Misc;
using ProtocolHelper.DataFieldTypes;

namespace Agent.Core.Proxy
{
    public enum ChannelSize
    {
        Double,     // 8 bytes
        Full,       // 4 bytes
        Half        // 2 bytes
    }

    public class RawDataLocation
    {
        public string Name;

        public uint PartialIndex; // corresponds to individual message codes - 0xD0

        public uint Offset; // in words (16 bits) 

        public ChannelSize Size; // determines whether the type size

        public bool IsReal;  // interpret as int or floating point (real) number

        public RawDataLocation(string name, uint partialIndex, uint offset, ChannelSize size, bool isReal)
        {
            Name = name;
            PartialIndex = partialIndex;
            Offset = offset;
            Size = size;
            IsReal = isReal;
        }

        public static double? GetRawData(FullDataReading reading, RawDataLocation location)
        {
            double? value = null;

            foreach (PartialRawData data in reading.RawDatas)
            {
                //                if ((data.Code - 0xD0) == location.PartialIndex)
                if ((data.Code - 0xF8) == location.PartialIndex)
                {
                    uint offset = location.Offset * 2;

                    switch (location.Size)
                    {
                        case (ChannelSize.Full):

                            FloatField field = new FloatField(true);

                            field.UnPack(data.RawData, ref offset);

                            value = field.Value;

                            break;
                        case (ChannelSize.Half):

                            UInt16DataField shortField = new UInt16DataField(true);

                            shortField.UnPack(data.RawData, ref offset);

                            value = shortField.Value;

                            break;
                        case (ChannelSize.Double):
                            throw new NotImplementedException("no double sizes supported yet");
                    }
                }
            }

            return value;
        }
    }
}
