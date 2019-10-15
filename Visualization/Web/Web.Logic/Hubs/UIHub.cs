namespace Web.Logic.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    using Common.Communication;
    using Common.Models;
    using Common.Models.Exceptions;

    using Web.Logic.Services;

    public class UIHub : Hub
    {
        private readonly ILog Log;
        private readonly IUnityAppManager appManager;

        public UIHub(ILog log, IUnityAppManager appManager) : base()
        {
            this.Log = log;
            this.appManager = appManager;
        }

        public async Task SignalForVehiclePopulation(VehiclePopulation population)
        {
            try
            {
                Log.Debug(population);
                await Task.Run(() =>
                {
                    Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured while sending statistics: {ex.Message}");
                throw new SignalHubException($"Error in SignalForVehiclePopulation method", ex);
            }
        }

        public Task SignalForUnityAppConnectionStatus(bool isConnected)
        {
            try
            {
                appManager.CheckStatus(isConnected);
                return Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, isConnected);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending Unity App connection status {ex.Message}");
                throw new SignalHubException($"Error in SignalForVehiclePopulation method", ex);
            }
        }
    }
}
