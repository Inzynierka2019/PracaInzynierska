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
        private readonly double latitudeReference = 54.373189;
        private readonly double longitudeReference = 18.609265;

        public StatisticsService(ILog Log, IHubContext<UIHub> hubContext)
        {
            this.Log = Log;
            this.hubContext = hubContext;
            this.vehiclePopulationList = new List<VehiclePopulation>();
        }

        public async Task UpdateVehiclePopulation(VehiclePopulation population)
        {
            foreach (var geoPosition in population.VehiclePositions)
            {
                var offset = CalcDecimalDegreesFromMeters(
                    latitude: this.latitudeReference, 
                    longitude: this.longitudeReference, 
                    x: geoPosition.Latitude, 
                    y: geoPosition.Longitude);

                geoPosition.Latitude = offset.Item1;
                geoPosition.Longitude = offset.Item2;
            }
            this.Log.Debug(population);
            this.vehiclePopulationList.Add(population);
            await this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
        }

        Tuple<double, double> CalcDecimalDegreesFromMeters(double latitude, double longitude, double x, double y)
        {
            //Earth’s radius, sphere
            var R = 6378137;

            //Coordinate offsets in radians
            var dLat = x / R;
            var dLon = y / (R * Math.Cos(Math.PI * latitude / 180));

            //OffsetPosition, decimal degrees
            var lat = latitude + dLat * 180 / Math.PI;
            var lon = longitude + dLon * 180 / Math.PI;

            return new Tuple<double, double>(lat, lon);
        }
    }
}
