using System;
using System.Text;

namespace Agent.Common.TypeExtensions
{
    public static class StringExtensions
    {
        public static string RemoveNonPrintableCharacters(this string source)
        {
            var builder = new StringBuilder();

            if (source != null)
            {
                foreach (char c in source)
                {
                    var b = (byte) c;

                    if (b < 127 && b > 31)
                    {
                        builder.Append(c);
                    }
                }
            }
            else
            {
                return null;
            }

            return builder.ToString();
        }

        /// <summary>
        ///     Remove characters from the front if string is longer than maxLength.
        /// </summary>
        public static string LeftTrimToMaxLength(this string stringToTrim, int maxLength)
        {
            string newString = stringToTrim;

            if (stringToTrim.Length > maxLength)
            {
                newString = stringToTrim.Remove(0, stringToTrim.Length - maxLength);
            }

            return newString;
        }

        /// <summary>
        ///     Does compare using rule
        /// </summary>
        /// <param name="source">string source</param>
        /// <param name="toCheck">string to compare</param>
        /// <param name="comp">Rule to use for comparison</param>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string GetResultPropertyAsString(this object result)
        {
            return result.GetType().GetProperty("Result").GetValue(result, null) as string;
        }

        public static string EnforceMaxLengthWithMessage(this string stringToTrim, int maxLength,
            string truncationMessage)
        {
            if (stringToTrim.Length > maxLength)
            {
                stringToTrim = stringToTrim.Substring(0, maxLength - truncationMessage.Length);

                return stringToTrim + truncationMessage;
            }

            return stringToTrim;
        }

        /// <summary>
        ///     Converts string to ascii bytes and compresses bytes using System.IO.Compression.GZipStream.
        /// </summary>
        /// <param name="uncompressed"></param>
        /// <returns></returns>
        public static byte[] CompressGZip_ASCII(this string uncompressed)
        {
            byte[] uncompressedBytes = Encoding.ASCII.GetBytes(uncompressed);

            return uncompressedBytes.CompressGZip();
        }
    }
}