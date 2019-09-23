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
        private readonly UnityAppCommunicationManager communicationManager;

        public UIHub(ILog log, UnityAppCommunicationManager communicationManager) : base()
        {
            this.Log = log;
            this.communicationManager = communicationManager;
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
                Log.Error($"Exception occured while sending statistics: {ex.Message}");
                throw new SignalHubException($"Error in SignalForVehiclePopulation method", ex);
            }
        }

        public Task SignalForUnityAppConnectionStatus(bool isConnected)
        {
            try
            {
                communicationManager.CheckStatus(isConnected);
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
