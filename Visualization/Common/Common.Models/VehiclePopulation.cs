using System;

namespace Common.Models
{
    public class VehiclePopulation
    {
        public string Title = "Vehicle population";
        public int[] Data { get; set; }
        public string Label { get; set; } = DateTime.Now.ToString("mm:ss");

        public override string ToString()
        {
            return $"[{Label}] {Title}: {string.Join(",", Data)}";
        }
    }
}
