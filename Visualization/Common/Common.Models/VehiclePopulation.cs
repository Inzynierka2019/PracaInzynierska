using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class VehiclePopulation : IMessage
    {
        public string Title = "Vehicle population";
        public DateTime timestamp = DateTime.Now;
        /// <summary>
        /// item1 = position.x
        /// item2 = position.y
        /// item3 = vehicle.id
        /// </summary>
        public List<Tuple<float, float, int>> vehiclePositions = new List<Tuple<float, float, int>>();

        public override string ToString()
        {
            return $"{Title} with {vehiclePositions.Count} objects";
        }
    }
}
