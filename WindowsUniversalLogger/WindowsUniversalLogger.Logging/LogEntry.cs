using System;
using WindowsUniversalLogger.Interfaces;

namespace WindowsUniversalLogger.Logging
{
    public class LogEntry : ILogEntry
    {
        public LogEntry(LogLevel logLevel, string message)
        {
            this.Time = DateTime.Now;
            this.LogLevel = logLevel;
            this.Message = message;
        }

        public LogEntry(LogLevel logLevel, string message, params object[] args)
        {
            this.Time = DateTime.Now;
            this.LogLevel = logLevel;
            this.Message = string.Format(message, args);
        }

        public LogLevel LogLevel { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }
}
