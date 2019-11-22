namespace Web.Logic.WebUI
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    using Common.Models;
    using Common.Models.Enums;
    using Web.Logic.Hubs;
    using Serilog;

    public class ConsoleLogUpdater : IConsoleLogUpdater
    {
        private readonly IHubContext<UIHub> hub;

        public ConsoleLogUpdater(IHubContext<UIHub> hub)
        {
            this.hub = hub;
        }

        public async Task SendConsoleLog(string message, LogType type)
        {
            try
            {
                await this.hub.Clients.All.SendAsync(
                    SignalMethods.SignalForConsoleLogs.Method,
                    new ConsoleLog(message, type));
            }
            catch (Exception e)
            {
                Log.Error($"Exception was thrown while sending Console Log messages", e);
            }

        }
    }
}
