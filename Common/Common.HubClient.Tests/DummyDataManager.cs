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
        private static int InitialCount { get; set; }

        static DummyDataManager()
        {
            var r = new Random();
            InitialCount = r.Next(100, 600);
        }

        public static VehiclePopulation GetVehiclePopulation()
        {
            var r = new Random();
            var dev = r.Next(0, 25);

            return new VehiclePopulation
            {
                Count = InitialCount + r.Next(-dev, dev)
            };
        }
    }
}
