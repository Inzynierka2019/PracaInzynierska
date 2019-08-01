namespace Web.Logic.Services
{
    using Common.Models.Enums;
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
            if ((string)message != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message as string, type);
            }
        }

        public void Error(object message, LogType type)
        {
            if ((string)message != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message as string, type);
            }
        }

        public void Warn(object message, LogType type)
        {
            if ((string)message != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message as string, type);
            }
        }

        public void Debug(object message, LogType type)
        {
            if ((string)message != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message as string, type);
            }
        }

        public void Info(object message, LogType type)
        {
            if ((string)message != "" && message != null)
            {
                log.Debug(message);
                this.consoleLog.SendConsoleLog(message as string, type);
            }
        }
    }
}
