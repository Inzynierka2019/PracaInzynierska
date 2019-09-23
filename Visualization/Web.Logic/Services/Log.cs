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

        public void Fatal(object message)
        {
            if (message as string != "" && message != null)
            {
                log.Fatal(message);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Fatal);
            }
        }

        public void Error(object message)
        {
            if (message as string != "" && message != null)
            {
                log.Error(message);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Error);
            }
        }

        public void Warn(object message)
        {
            if (message as string != "" && message != null)
            {
                log.Warn(message);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Warning);
            }
        }

        public void Debug(object message)
        {
            if (message as string != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Debug);
            }
        }

        public void Info(object message)
        {
            if (message as string != "" && message != null)
            {
                log.Info(message);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Info);
            }
        }

        public void Success(object message)
        {
            if (message as string != "" && message != null)
            {
                log.Info(message);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Success);
            }
        }
    }
}
