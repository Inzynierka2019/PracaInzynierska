using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
    /// <summary>
    /// ILogger
    /// </summary>
    public class Logger : ILogger
    {
        private readonly IHubContext<ConsoleHub> hub;

        private int Timeout { get; } = 500;

        private string SysConsole { get; } = "NotifySystemConsole";

        private string SimConsole { get; } = "NotifySimConsole";


        public Logger(IHubContext<ConsoleHub> hub)
        {
            this.hub = hub;
        }

        public void Info(string message)
        {
            this.LogMessage(this.SimConsole, message, LogType.INFO);

        }

        public void Debug(string message)
        {
            this.LogMessage(this.SimConsole, message, LogType.NORMAL);
        }

        public void Error(string message)
        {
            this.LogMessage(this.SysConsole, message, LogType.ERROR);
        }

        public void Warning(string message)
        {
            this.LogMessage(this.SysConsole, message, LogType.WARNING);
        }

        private async void LogMessage(string channel, string message, LogType type)
        {
            await Task.Run(() => {
                Thread.Sleep(this.Timeout);
                hub.Clients.All.SendAsync(channel, message, type);
            });
        }
    }
}
