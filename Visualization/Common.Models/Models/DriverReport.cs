using Common.Models.Models;
using System;

namespace Common.Models
{
    public class DriverReport : IMessage
    {
        public float AvgSpeed { get; set; }
        public TimeSpan TravelTime { get; set; }
        public IDriver Driver { get; set; }
        public string RouteTarget { get; set; }
    }
}
