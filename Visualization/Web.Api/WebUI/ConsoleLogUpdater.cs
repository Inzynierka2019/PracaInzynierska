namespace Web.Api.WebUI
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    using log4net;

    using Common.HubClient;
    using Common.Models;
    using Common.Models.Enums;
    using Web.Api.Hubs;

    public class ConsoleLogUpdater : IConsoleLogUpdater
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConsoleLogUpdater));

        private readonly IHubContext<UIHub> hub;

        public ConsoleLogUpdater(IHubContext<UIHub> hub)
        {
            this.hub = hub;
        }

        public async Task SendConsoleLog(string message, LogMessageType type)
        {
            try
            {
                var log = new ConsoleLog(message, type);

                await Task.Run(() =>
                {
                    this.hub.Clients.All.SendAsync(SignalMethods.SignalForConsoleLogs.Method, log);
                });
            }
            catch (Exception e)
            {
                Log.Error("Exception was thrown while sending Console Log messages", e);
            }

        }
    }
}
