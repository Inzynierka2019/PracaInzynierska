using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class VehiclePopulation : IMessage
    {
        public DateTime TimeStamp = DateTime.Now;
        public List<GeoPosition> VehiclePositions = new List<GeoPosition>();
        public int VehicleCount => VehiclePositions.Count;

        public override string ToString()
        {
            return $"[{TimeStamp.ToLongTimeString()}] {VehicleCount} object(s)";
        }
    }
}
