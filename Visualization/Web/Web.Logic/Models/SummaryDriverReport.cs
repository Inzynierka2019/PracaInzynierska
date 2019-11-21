using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Logic.Models
{
    public class SummaryDriverReport
    {
        public TimeSpan TravelTime { get; set; }
        public Dictionary<Personality, DriverStatistics> DriversByPersonality { get; set; }
        public string RouteTarget { get; set; }
        public DateTime TimeStamp = DateTime.Now;

        public SummaryDriverReport(List<DriverReport> driverReports)
        {
            this.DriversByPersonality = new Dictionary<Personality, DriverStatistics>();
            var drivers = new List<DriverStatistics>();

            foreach (var report in driverReports)
            {
                var driverStats = new DriverStatistics()
                {
                    AvgAge = report.Driver.Age,
                    AvgReactionTime = report.Driver.ReactionTime,
                    Personality = report.Driver.Personality,
                    AvgSpeed = report.AvgSpeed,
                    AvgTravelTime = report.TravelTime,
                    MostPopularRouteTarget = report.RouteTarget
                };

                drivers.Add(driverStats);
            }

            var groupedDrivers = drivers.GroupBy(x => x.Personality);

            foreach (var group in groupedDrivers)
            {
                var targetRoute = group.Select(x => x.MostPopularRouteTarget);
                var debugA = targetRoute.GroupBy(s => s);
                var debugC = debugA.OrderByDescending(i => i.Count());
                var debugD =  debugC.Select(t => t.Key).FirstOrDefault();
                var ageAvg = group.Select(x => x.AvgAge).Average();
                var speedAvg = group.Select(x => x.AvgSpeed).Average();
                var ReactionTimeAvg = group.Select(x => x.AvgReactionTime).Average();
                double doubleAverageTicks = group.Select(x => x.AvgTravelTime).Average(timespan => timespan.Ticks);
                long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                var travelTimeAvg = new TimeSpan(longAverageTicks);
               
                this.DriversByPersonality.Add(group.Key, new DriverStatistics()
                {
                    Personality = group.Key,
                    DriverType = group.Key.ToString(),
                    AvgAge = (int)ageAvg,
                    AvgReactionTime = ReactionTimeAvg,
                    AvgSpeed = (int)speedAvg,
                    AvgTravelTime = travelTimeAvg,
                    MostPopularRouteTarget = debugD,
                    Count = group.Count()
                });
            }
        }
    }
}
