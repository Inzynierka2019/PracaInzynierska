namespace Web.Logic.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    using Common.Communication;
    using Common.Models;
    using Common.Models.Enums;
    using Common.Models.Exceptions;

    using Web.Logic.Services;

    public class UIHub : Hub
    {
        private readonly ILog Log;

        public UIHub(ILog log) : base()
        {
            this.Log = log;
        }

        public Task SignalForVehiclePopulation(VehiclePopulation population)
        {
            try
            {
                Log.Debug(population);
                return Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured while sending statistics: {ex.Message}");
                throw new SignalHubException($"Error in SignalForVehiclePopulation method", ex);
            }
        }
    }
}
