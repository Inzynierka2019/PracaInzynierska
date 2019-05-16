using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using Web.Services.Hubs;

namespace Web.Services
{
    /// <summary>
    /// ILogger
    /// </summary>
    public class SignalLogger<T> : ISignalLogger<T> where T : Hub
    {
        private readonly IHubContext<T> hub;

        private int Timeout { get; } = 1000;

        private string ConsoleName { get; }

        public SignalLogger(IHubContext<T> hub)
        {
            this.hub = hub;
            this.ConsoleName = typeof(T).Name;
        }

        public void Info(string message)
        {
            this.LogMessage(this.ConsoleName, message, LogType.INFO);
        }

        public void Debug(string message)
        {
            this.LogMessage(this.ConsoleName, message, LogType.NORMAL);
        }

        public void Error(string message)
        {
            this.LogMessage(this.ConsoleName, message, LogType.ERROR);
        }

        public void Warning(string message)
        {
            this.LogMessage(this.ConsoleName, message, LogType.WARNING);
        }

        private async void LogMessage(string channel, string message, LogType type)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(this.Timeout);
                hub.Clients.All.SendAsync(channel, message, type);
            });
        }
    }
}
