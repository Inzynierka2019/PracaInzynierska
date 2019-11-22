namespace Web.Logic.Services
{
    using Common.Models.Enums;
    using System;
    using Web.Logic.WebUI;

    public class Log : ILog
    {
        private readonly IConsoleLogUpdater consoleLog;

        public Log(IConsoleLogUpdater consoleLog)
        {
            this.consoleLog = consoleLog;
        }

        public void Fatal(object message)
        {
            if (message as string != "" && message != null)
            {
                Serilog.Log.Fatal(message as string);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Fatal);
            }
        }

        public void Error(object message)
        {
            if (message as string != "" && message != null)
            {
                Serilog.Log.Error(message as string);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Error);
            }
        }

        public void Warn(object message)
        {
            if (message as string != "" && message != null)
            {
                Serilog.Log.Warning(message as string);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Warning);
            }
        }

        public void Debug(object message)
        {
            if (message as string != "" && message != null)
            {
                Serilog.Log.Debug(message as string);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Debug);
            }
        }

        public void Info(object message)
        {
            if (message as string != "" && message != null)
            {
                Serilog.Log.Information(message as string);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Info);
            }
        }

        public void Success(object message)
        {
            if (message as string != "" && message != null)
            {
                Serilog.Log.Information(message as string);
                this.consoleLog.SendConsoleLog(message.ToString(), LogType.Success);
            }
        }
    }
}
