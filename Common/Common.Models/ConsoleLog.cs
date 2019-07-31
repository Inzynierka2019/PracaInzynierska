namespace Common.Models
{
    using System;
    using Common.Models.Enums;

    public class ConsoleLog
    {
        public string Message { get; set; }

        public DateTime TimeStamp { get; } = DateTime.Now;

        public LogMessageType Type { get; set; }

        public ConsoleLog() { }

        public ConsoleLog(string message, LogMessageType type)
        {
            this.Message = message;
            this.Type = type;
        }
    }
}
