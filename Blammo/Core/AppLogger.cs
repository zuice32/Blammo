using System;
using System.Globalization;
using System.IO;

namespace Agent.Core
{
    public static class AppLogger
    {
        private const int MaxSizeBytes = 2000000;  // 2 MB

#if WindowsCE
        private static readonly string PathToAppDataDirectory = "";   
#else
        private static readonly string PathToAppDataDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
#endif

        private const string GeoNetDirectory = "Geokon\\GeoNet";

        private const string GeoNetAppLogFileName = "applog.txt";

        private const string GeoNetAppLogBakFileName = "applog.bak";

        private static void EnforceMaximumLogSize()
        {
            string directory = Path.Combine(PathToAppDataDirectory, GeoNetDirectory);

            DirectoryInfo di = new DirectoryInfo(directory);

            foreach (FileInfo fileInfo in di.GetFiles())
            {
                if (fileInfo.Name != GeoNetAppLogFileName) continue;

                if (fileInfo.Length <= MaxSizeBytes) continue;

                string bakLogFileName = Path.Combine(PathToAppDataDirectory, GeoNetDirectory, GeoNetAppLogBakFileName);
                string logFileName = Path.Combine(PathToAppDataDirectory, GeoNetDirectory, GeoNetAppLogFileName);

                try
                {
                    if (File.Exists(bakLogFileName))
                    {
                        File.Delete(bakLogFileName);
                    }

                    File.Copy(logFileName, bakLogFileName);

                    File.Delete(logFileName);
                }
// ReSharper disable once EmptyGeneralCatchClause
                catch 
                {
                }
            }
        }


        private static StreamWriter LogStream()
        {
            EnforceMaximumLogSize();

            string logPath = Path.Combine(PathToAppDataDirectory, GeoNetDirectory, GeoNetAppLogFileName);
        
            return new StreamWriter(logPath, true);
        }

        public static void WriteNoTime(string output)
        {
            using (StreamWriter sw = LogStream())
            {
                sw.WriteLine(output);
            }    
        }

        public static void WriteLine(string output)
        {
            using (StreamWriter sw = LogStream())
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture);
                sw.WriteLine(timestamp + " " + output);
            }   
        }

        public static void Write(string output)
        {
            using (StreamWriter sw = LogStream())
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture);
                sw.Write(timestamp + " " + output);
            }
        }

        public static void WriteAndLogException(Exception e)
        {
            WriteLine(e.ToString());
            Console.WriteLine(e.ToString());
        }
    }
}
