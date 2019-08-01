namespace Common.Models
{
    using System;
    using Common.Models.Enums;

    public class ConsoleLog
    {
        public string Message { get; set; }

        public string TimeStamp { get; } = DateTime.Now.ToString("hh:mm:ss");

        public LogType LogType { get; set; }

        public ConsoleLog() { }

        public ConsoleLog(string message, LogType type)
        {
            this.Message = message;
            this.LogType = type;
        }
    }
}
