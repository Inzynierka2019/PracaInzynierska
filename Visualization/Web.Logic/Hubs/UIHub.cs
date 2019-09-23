namespace Web.Logic.Hubs
{
    using Common.HubClient;
    using Common.Models;
    using Common.Models.Enums;
    using Common.Models.Exceptions;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Threading.Tasks;
    using Web.Logic.Services;

    public class UIHub : Hub
    {
        private readonly ILog Log;

        public UIHub(ILog log) : base()
        {
            this.Log = log;
        }

        public async Task SignalForVehiclePopulation(VehiclePopulation population)
        {
            try
            {
                Log.Debug(population, LogType.Debug);
                await Task.Run(() =>
                {
                    Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured while sending statistics: {ex.Message}", LogType.Error);
                throw new SignalHubException($"Error in SignalForVehiclePopulation method", ex);
            }
        }
    }
}
