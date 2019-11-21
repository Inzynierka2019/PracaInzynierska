using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Logic.Models
{
    public class DriverStatistics
    {
        public Personality Personality { get; set; }
        public string DriverType { get; set; }
        public float AvgReactionTime { get; set; }
        public float AvgAge { get; set; }
        public float AvgSpeed { get; set; }
        public TimeSpan AvgTravelTime { get; set; }
        public string AvgTravelTimeText { get; set; }
        public string MostPopularRouteTarget { get; set; }
        public int Count { get; set; }
    }
}
