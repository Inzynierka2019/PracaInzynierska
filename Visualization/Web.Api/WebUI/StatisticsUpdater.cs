using log4net;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Hubs;

namespace Web.Api.WebUI
{
    public class StatisticsUpdater
    {
        private const string SignalForVehiclePopulation = "VehiclePopulation";

        private static readonly ILog Log = LogManager.GetLogger(typeof(StatisticsUpdater));

        private readonly IHubContext<UIHub> hub;

        public StatisticsUpdater(IHubContext<UIHub> hub)
        {
            this.hub = hub;
        }

        public void SendVehiclePopulation()
        {
            try
            {
                this.hub.Clients.All.SendAsync()
            }
            catch (Exception ex)
            {
                Log.Error("Exception occured while sending statistics", ex);
            }
        }
    }
}
