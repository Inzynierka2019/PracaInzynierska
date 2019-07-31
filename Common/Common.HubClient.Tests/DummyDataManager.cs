using Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.HubClient.Tests
{
    public static class DummyDataManager
    {
        public static VehiclePopulation GetVehiclePopulation(int min = 200, int max = 600)
        {
            var r = new Random();
            int count = (int)(min + (double)Process.GetCurrentProcess().StartTime.Millisecond * (double)(max-min) / 1000.0);

            return new VehiclePopulation
            {
                Count = count + r.Next(-50, 50)
            };
        }
    }
}
