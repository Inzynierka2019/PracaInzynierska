namespace Common.Models
{
    using System;
    using Common.Models.Enums;

    public class ConsoleLog
    {
        public string Message { get; set; }

        public DateTime TimeStamp { get; } = DateTime.Now;

        public LogType Type { get; set; }

        public ConsoleLog() { }

        public ConsoleLog(string message, LogType type)
        {
            this.Message = message;
            this.Type = type;
        }
    }
}
