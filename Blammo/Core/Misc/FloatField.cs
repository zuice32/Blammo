using System;
using ProtocolHelper.DataFieldTypes;

namespace Agent.Core.Misc
{
    public class FloatField : DataFieldBase<float>
    {
        public FloatField(bool bigEndian)
        {
            BigEndian = bigEndian;

        }

        public UInt16 LSW
        {
            get
            {
                byte[] asArray = new byte[sizeof(float)];

                uint offset = 0;

                Pack(asArray, ref offset);

                return (UInt16)((asArray[0] << 8) + asArray[1]);
            }
        }
        public UInt16 MSW
        {
            get
            {
                byte[] asArray = new byte[sizeof(float)];

                uint offset = 0;

                Pack(asArray, ref offset);

                return (UInt16)((asArray[2] << 8) + asArray[3]);
            }
        }
        public override uint Length
        {
            get { return sizeof(float); }
        }

        public override void Pack(byte[] array, ref uint offset)
        {
            byte[] valueAsBytes = BitConverter.GetBytes(Value);

            if (BigEndian)
            {
                array[offset++] = valueAsBytes[1];
                array[offset++] = valueAsBytes[0];
                array[offset++] = valueAsBytes[3];
                array[offset++] = valueAsBytes[2];
            }
            else
            {
                array[offset++] = valueAsBytes[0];
                array[offset++] = valueAsBytes[1];
                array[offset++] = valueAsBytes[2];
                array[offset++] = valueAsBytes[3];

            }
        }

        public override void UnPack(byte[] array, ref uint offset)
        {
            byte[] valueAsBytes = new byte[sizeof(float)];

            if (BigEndian)
            {
                valueAsBytes[1] = array[offset++];
                valueAsBytes[0] = array[offset++];
                valueAsBytes[3] = array[offset++];
                valueAsBytes[2] = array[offset++];

            }
            else
            {
                valueAsBytes[2] = array[offset++];
                valueAsBytes[3] = array[offset++];
                valueAsBytes[0] = array[offset++];
                valueAsBytes[1] = array[offset++];

            }

            Value = BitConverter.ToSingle(valueAsBytes, 0);
        }
    }
}
