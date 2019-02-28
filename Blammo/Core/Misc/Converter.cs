using System.Text;

namespace Agent.Core.Misc
{
    public class Converter
    {
        public static string ByteArrayToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in data)
            {
                sb.AppendFormat(@"{0:X} ", b);
            }

            return sb.ToString().Trim(' ');
        }
    }
}
