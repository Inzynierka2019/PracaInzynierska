namespace Web.Logic.Services
{
    using Common.Models;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.Logic.Hubs;

    public class StatisticsService : IStatisticsService
    {
        private readonly ILog Log;
        private readonly IHubContext<UIHub> hubContext;
        private readonly List<VehiclePopulation> vehiclePopulationList;

        public StatisticsService(ILog Log, IHubContext<UIHub> hubContext)
        {
            this.Log = Log;
            this.hubContext = hubContext;
            this.vehiclePopulationList = new List<VehiclePopulation>();
        }

        public async Task UpdateVehiclePopulation(VehiclePopulation population)
        {
            var lat = 111320;
            var lon = 40075000 * Math.Cos(lat) / 360;

            foreach (var geoPosition in population.VehiclePositions)
            {
                geoPosition.Latitude = 54.373189 + geoPosition.Latitude / lat;
                geoPosition.Longitude = 18.609265 + geoPosition.Longitude / lon;
            }
            this.Log.Debug(population);
            this.vehiclePopulationList.Add(population);
            await this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
        }
    }
}
