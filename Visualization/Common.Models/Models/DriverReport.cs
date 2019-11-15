using System;

namespace Common.Models
{
    public class DriverReport : IMessage
    {
        public float AvgSpeed { get; set; }
        public TimeSpan TravelTime { get; set; }
        public Driver Driver { get; set; }
        public string RouteTarget { get; set; }
        public DateTime TimeStamp = DateTime.Now;

        public override string ToString()
        {
            return $"Time: {TravelTime} to \"{RouteTarget}\"";
        }
    }
}
