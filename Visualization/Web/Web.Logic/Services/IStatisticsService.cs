using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Logic.Models;

namespace Web.Logic.Services
{
    public interface IStatisticsService
    {
        void InitializeStatisticsService();
        Task UpdateVehiclePopulation(VehiclePopulation population);
        Task UpdatePersonalityStats(VehiclePopulation population);
        Task UpdateMomentarySpeeds(VehiclePopulation population);
        void UpdateDriverReports(DriverReport report);
        IEnumerable<DriverStatistics> GetDriverReports();
    }
}
