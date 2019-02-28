using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtocolHelper.DataFieldTypes;

namespace Agent.Core.Proxy
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PartialRawData : DataReadingBase, IDataField
    {
        [JsonProperty] public byte[] RawData = new byte[20];

        public PartialRawData()
        {
            BigEndian = true;
        }

        public uint Length { get; private set; }

        public bool BigEndian { get; set; }

        public void Pack(byte[] array, ref uint offset)
        {
            throw new NotImplementedException();
        }

        public void UnPack(byte[] array, ref uint offset)
        {
            UInt16DataField codeAndIndex = new UInt16DataField(true);
            LongDataField serialNumber = new LongDataField(Int32ByteOrder.BADC);
            GeokonDateTimeField time = new GeokonDateTimeField();

            codeAndIndex.UnPack(array, ref offset);

            Code = (uint) (codeAndIndex.Value & 0x00FF);

//            if (Code < 0xD0 || Code > 0xD8)
//                throw new Exception("using the wrong method for this version of firmware");

//            Version = (ushort)(codeAndIndex.Value >> 8);  // not valid for code 0xD0


            // temporary until firmware is changed on all nodes ============


            if (Code < 0xD0 || Code > 0xD8)
            {
                byte[] temp = new byte[2];

                Version = (ushort) (temp[0]*256 + temp[1]);

                Buffer.BlockCopy(array, (int) offset, temp, 0, temp.Length); // save bytes to temp

                Buffer.BlockCopy(array, (int) offset + temp.Length, array, (int) offset, 26);

                Buffer.BlockCopy(temp, 0, array, (int) offset + 26, temp.Length);
            }

            // ================================================

            serialNumber.UnPack(array, ref offset);
            SerialNumber = serialNumber.Value;

            time.UnPack(array, ref offset);
            SampleTime = time.Value;

            for (int x = 0; x < RawData.Length; x++)
            {
                RawData[x] = array[offset++];
            }

        }
    }
}
