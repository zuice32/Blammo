using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Agent.Common.TypeExtensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] CompressGZip(this byte[] uncompressedBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream compressedzipStream = new GZipStream(memoryStream,
                                                                       CompressionMode.Compress,
                                                                       true))
                {
                    compressedzipStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        public static string ToASCIIString(this byte[] target)
        {
            return new ASCIIEncoding().GetString(target, 0, target.Length);
        }

        public static byte[] TrimBothEnds(this byte[] bytes, int trimFromLeft, int trimFromRight)
        {
            int count = bytes.Length - trimFromLeft - trimFromRight;

            byte[] result = new byte[count];

            Buffer.BlockCopy(bytes, trimFromLeft, result, 0, count);

            return result;
        }
        public static byte[] Append(this byte[]b, byte value)
        {
            return InsertAt(b, b.Length, value);
        }
        public static byte[] InsertAt(this byte[]b, int index, byte value)
        {
            if (index > b.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            byte[] result = new byte[b.Length + 1];

            if(index != 0)
            {
                Buffer.BlockCopy(b, 0, result, 0, index);
            }
            
            result[index] = value;

            Buffer.BlockCopy(b, index, result, index+1, b.Length - index);

            return result;
            
        }
        public static byte[] RemoveAt(this byte[]b, int index)
        {
            if(b.Length-1 < index)
            {
                throw new ArgumentOutOfRangeException();
            }
            byte[] result = new byte[b.Length -1];

            if(index == 0)
            {
                return TrimFromLeft(b, 1);
            }

            Buffer.BlockCopy(b,0,result,0,index);

            Buffer.BlockCopy(b, index + 1, result, index, b.Length - index - 1);

            return result;
        }
        public static byte[] TrimFromRight(this byte[] b, int numbytes)
        {
            if (numbytes > 0)
            {
                return b.TrimBothEnds(0, numbytes);
            }
            else
            {
                return b;
            }
        }

        public static byte[] TrimFromLeft(this byte[] b, int numbytes)
        {
            if (numbytes > 0 )
            {
                return b.TrimBothEnds(numbytes, 0);
            }
            else
            {
                return b;
            }
        }
    }
}
