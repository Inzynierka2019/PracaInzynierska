using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.HubClient.HubClient;
using Common.Models;

namespace Libraries.Web
{
    public class AppUpdater : IAppUpdater
    {
        private readonly IDebugLogger logger;
        public HubClient HubClient { get; set; }

        public AppUpdater(IDebugLogger logger, string address)
        {
            this.logger = logger;
            HubClient = new HubClient(logger, address, "AppUpdater");
        }

        #region Public Methods
        public void UpdateVehiclePopulationPositions(VehiclePopulation population)
        {
            //To do
        }
        #endregion
    }
}
