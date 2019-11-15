namespace Web.Logic.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    using Common.Models;
    using Common.Models.Exceptions;

    using Web.Logic.Services;
    using Common.Models.Enums;
    using System.Collections.Generic;
    using Common.Models;

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
                await Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured while sending statistics: {ex.Message}");
                throw new SignalHubException($"Error in SignalForVehiclePopulation method", ex);
            }
        }

        public async Task SignalForUnityAppConnectionStatus(UnityAppState state)
        {
            try
            {
                await Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, state);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending Unity App connection status {ex.Message}");
                throw new SignalHubException($"Error in SignalForUnityAppConnectionStatus method", ex);
            }
        }

        public async Task SignalForDriverReports(DriverReport driverReport)
        {
            try
            {
                await Clients.All.SendAsync(SignalMethods.SignalForDriverReports.Method, driverReport);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending driver report {ex.Message}");
                throw new SignalHubException($"Error in SignalForDriverReports method", ex);
            }
        }

        public async Task SignalForPersonalityStats(Dictionary<Personality, int> stats)
        {
            try
            {
                await Clients.All.SendAsync(SignalMethods.SignalForPersonalityStats.Method, stats);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending personality stats {ex.Message}");
                throw new SignalHubException($"Error in SignalForDriverReports method", ex);
            }
        }

        public async Task SignalForAvgSpeedByPersonality(Dictionary<Personality, float> stats)
        {
            try
            {
                await Clients.All.SendAsync(SignalMethods.SignalForAvgSpeedByPersonality.Method, stats);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending avg speed by personality stats {ex.Message}");
                throw new SignalHubException($"Error in SignalForDriverReports method", ex);
            }
        }
    }
}
