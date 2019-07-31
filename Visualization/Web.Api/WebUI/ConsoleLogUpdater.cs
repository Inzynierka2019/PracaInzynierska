namespace Web.Api.WebUI
{
    using log4net;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using Web.Api.Hubs;

    public class ConsoleLogUpdater
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConsoleLogUpdater));

        private readonly IHubContext<UIHub> hubContext;

        public ConsoleLogUpdater(IHubContext<UIHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void SendConsoleLogs(string message)
        {
            try
            {
                //this.hubContext.Clients.All.SendAsync(
                //    SimulationConsoleLogs,
                //    new { Message = message, Timestamp = DateTime.Now });
            }
            catch (Exception e)
            {
                Log.Error("Exception was thrown while sending Console Log messages", e);
            }

        }
    }
}
