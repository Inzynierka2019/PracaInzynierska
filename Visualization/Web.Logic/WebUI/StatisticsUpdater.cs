namespace Web.Logic.WebUI
{
    using System;
    using Microsoft.AspNetCore.SignalR;

    using Common.Models;
    using Web.Logic.Hubs;
    using Web.Logic.Services;
    using Common.Models.Enums;
    using Common.HubClient;

    public class StatisticsUpdater
    {
        private readonly IHubContext<UIHub> hub;
        private readonly ILog Log;

        public StatisticsUpdater(IHubContext<UIHub> hub, ILog log)
        {
            this.hub = hub;
            this.Log = log;
        }
    }
}
