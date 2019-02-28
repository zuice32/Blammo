using System.IO;

namespace Agent.Core.Logging
{
    public class TextFileLogWriter : ILogWriter
    {
        private readonly long _maxLogSizeBytes;
        private readonly string _pathToLog;

        public TextFileLogWriter(string pathToLog): this(pathToLog, 10000000)
        {
        }

        public TextFileLogWriter(string pathToLog, long maxLogSizeBytes)
        {
            _pathToLog = pathToLog;

            _maxLogSizeBytes = maxLogSizeBytes;
        }

        public void AddEntry(ILogEntry logEntry)
        {
            try
            {
                using (StreamWriter streamWriter = File.AppendText(_pathToLog))
                {
                    streamWriter.WriteLine(TextLogEntryFormatter.GetEntry(logEntry.Level,
                                                                          logEntry.Source,
                                                                          logEntry.Message));

                    streamWriter.Flush();
                    streamWriter.Close();
                }

                FileInfo fileInfo = new FileInfo(_pathToLog);
            
                if (fileInfo.Length > _maxLogSizeBytes)
                {
                    fileInfo.Delete();
                }
            }
            catch 
            {
                //ignore
            }
        }
    }
}
