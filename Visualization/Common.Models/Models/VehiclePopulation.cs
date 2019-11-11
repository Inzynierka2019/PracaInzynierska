using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class VehiclePopulation : IMessage
    {
        public DateTime TimeStamp = DateTime.Now;
        public List<VehicleStatus> VehiclePositions = new List<VehicleStatus>();
        public int VehicleCount => VehiclePositions.Count;

        public override string ToString()
        {
            return $"[{TimeStamp.ToLongTimeString()}] {VehicleCount} object(s)";
        }
    }
}
