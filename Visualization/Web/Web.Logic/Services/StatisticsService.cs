namespace Web.Logic.Services
{
    using Common.Models;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Logic.Hubs;
    using Web.Logic.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly ILog Log;
        private readonly IHubContext<UIHub> hubContext;
        private List<DriverReport> driverReports;
        private List<VehiclePopulation> vehiclePopulationList;
        private Dictionary<Personality, int> personalityStats;
        private Dictionary<Personality, float> avgSpeedByPersonality { get; set; }
        private readonly IProcessService processService;

        private SceneData sceneData;

        public StatisticsService(ILog Log, IHubContext<UIHub> hubContext, IProcessService processService)
        {
            this.Log = Log;
            this.hubContext = hubContext;
            this.processService = processService;
        }

        public void InitializeStatisticsService()
        {
            this.Log.Debug("Initialized statistics service.");
            this.vehiclePopulationList = new List<VehiclePopulation>();
            this.personalityStats = new Dictionary<Personality, int>();
            this.avgSpeedByPersonality = new Dictionary<Personality, float>();
            foreach (Personality personality in Enum.GetValues(typeof(Personality)))
            {
                this.personalityStats.Add(personality, 0);
                this.avgSpeedByPersonality.Add(personality, 0f);
            }
            this.driverReports = new List<DriverReport>();

            SimulationPreferences simulationPreferences = this.processService.GetJsonSimulationPreferences();
            this.sceneData = simulationPreferences.availableScenes.Find(x => x.scene == simulationPreferences.currentScene);
        }

        public IEnumerable<DriverStatistics> GetDriverReports()
        {
            if (this.driverReports.Count < 2)
            {
                return null;
            }

            return new SummaryDriverReport(this.driverReports).DriversByPersonality.Select(x => x.Value).ToArray();
        }

        public async Task UpdatePersonalityStats(VehiclePopulation population)
        {
            foreach (Personality personality in Enum.GetValues(typeof(Personality)))
            {
                this.personalityStats[personality] = 0;
            }

            var groupped = population.VehicleStatuses.GroupBy(x => x.Personality);

            foreach (var group in groupped)
            {
                this.personalityStats[group.Key] = group.Count();
            }

            await this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForPersonalityStats.Method, this.personalityStats);
        }

        public void UpdateDriverReports(DriverReport report)
        {
            this.driverReports.Add(report);
        }

        public async Task UpdateMomentarySpeeds(VehiclePopulation population)
        {
            foreach (Personality personality in Enum.GetValues(typeof(Personality)))
            {
                this.avgSpeedByPersonality[personality] = 0;
            }

            var groupped = population.VehicleStatuses.GroupBy(x => x.Personality);

            foreach(var group in groupped)
            {
                this.avgSpeedByPersonality[group.Key] = group.Select(x => x.CurrentSpeed).Average();
            }

            await this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForAvgSpeedByPersonality.Method, this.avgSpeedByPersonality);
        }

        public async Task UpdateVehiclePopulation(VehiclePopulation population)
        {
            foreach (var geoPosition in population.VehicleStatuses)
            {
                var offset = CalcDecimalDegreesFromMeters(
                    latitude: sceneData.latitude, 
                    longitude: sceneData.longitude,
                    x: geoPosition.Latitude, 
                    y: geoPosition.Longitude);

                geoPosition.Latitude = offset.Item1;
                geoPosition.Longitude = offset.Item2;
            }

            this.vehiclePopulationList.Add(population);
            await this.hubContext.Clients.All.SendAsync(SignalMethods.SignalForVehiclePopulation.Method, population);
        }

        private Tuple<double, double> CalcDecimalDegreesFromMeters(double latitude, double longitude, double x, double y)
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
