namespace Web.Logic.Services
{
    using Common.Models.Enums;
    using System;
    using Web.Logic.WebUI;

    public class Log : ILog
    {
        private readonly IConsoleLogUpdater consoleLog;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Log));

        public Log(IConsoleLogUpdater consoleLog)
        {
            this.consoleLog = consoleLog;
        }

        public void Fatal(object message, LogType type)
        {
            if (message as string != "" && message != null)
            {
                log.Fatal(message);
                this.consoleLog.SendConsoleLog(message.ToString(), type);
            }
        }

        public void Error(object message, LogType type)
        {
            if (message as string != "" && message != null)
            {
                log.Error(message);
                this.consoleLog.SendConsoleLog(message.ToString(), type);
            }
        }

        public void Warn(object message, LogType type)
        {
            if (message as string != "" && message != null)
            {
                log.Warn(message);
                this.consoleLog.SendConsoleLog(message.ToString(), type);
            }
        }

        public void Debug(object message, LogType type)
        {
            if (message as string != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message.ToString(), type);
            }
        }

        public void Info(object message, LogType type)
        {
            if (message as string != "" && message != null)
            {
                log.Info(message);
                this.consoleLog.SendConsoleLog(message.ToString(), type);
            }
        }
    }
}
