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
            InitialCount = r.Next(0, 1000);
        }

        public static VehiclePopulation GetVehiclePopulation()
        {
            var r = new Random();
            var dev = r.Next(0, 500);
            var newCount = InitialCount + r.Next(-dev, dev);
            if (newCount < 0) newCount = Math.Abs(dev);

            return new VehiclePopulation
            {
                Data = new int[] { newCount }
            };
        }
    }
}
