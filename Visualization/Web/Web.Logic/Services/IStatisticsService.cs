using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Logic.Services
{
    public interface IStatisticsService
    {
        Task UpdateVehiclePopulation(VehiclePopulation population);
    }
}
