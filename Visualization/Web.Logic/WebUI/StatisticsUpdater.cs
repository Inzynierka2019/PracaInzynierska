namespace Web.Logic.WebUI
{
    using System;
    using Microsoft.AspNetCore.SignalR;

    using Common.Models;
    using Web.Logic.Hubs;
    using Web.Logic.Services;
    using Common.Models.Enums;

    public class StatisticsUpdater
    {
        private readonly IHubContext<UIHub> hub;
        private readonly ILog Log;

        public StatisticsUpdater(IHubContext<UIHub> hub, ILog log)
        {
            this.hub = hub;
            this.Log = log;
        }

        public void SendVehiclePopulation(VehiclePopulation population)
        {
            try
            {
                //this.hub.Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation, population);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured while sending statistics: {ex.Message}", LogType.Error);
            }
        }
    }
}
