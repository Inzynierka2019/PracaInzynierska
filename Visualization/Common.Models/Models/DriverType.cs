using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Models
{
    public enum Personality
    {
        Slow,
        Normal,
        Aggresive,
    }
    public interface IDriver
    {
        Personality Personality {get; }
        float ReactionTime { get; set; }
        float Age { get; set; }
    }

    public static class DriverFactory
    {
        public static List<DriverSpawnChance> driverSpawnChances
            = new List<DriverSpawnChance>();
        public static IDriver CreateDriver(Personality personality)
        {
            switch (personality)
            {
                case Personality.Slow:
                    return new SlowDriver();
                case Personality.Normal:
                    return new NormalDriver();
                case Personality.Aggresive:
                    return new AggresiveDriver();
                default:
                    return null;
            }
        }

        public static IDriver GetRandomDriver()
        {
            int rand = new Random().Next(0, driverSpawnChances.Select(e => e.spawnChance).Sum());
            foreach(var driverSpawnChance in driverSpawnChances)
            {
                rand -= driverSpawnChance.spawnChance;
                if (rand <= 0)
                    return CreateDriver(driverSpawnChance.personality);
            }

            return null;
        }
    }

    public class SlowDriver : IDriver
    {
        public Personality Personality { get; } = Personality.Slow;
        public float ReactionTime { get; set; } = 0.2f;
        public float Age { get; set; } = new Random().Next(50, 80);
    }

    public class NormalDriver : IDriver
    {
        public Personality Personality { get; } = Personality.Normal;
        public float ReactionTime { get; set; } = 0.1f;
        public float Age { get; set; } = new Random().Next(25, 60);
    }

    public class AggresiveDriver : IDriver
    {
        public Personality Personality { get; } = Personality.Aggresive;
        public float ReactionTime { get; set; } = 0.5f;
        public float Age { get; set; } = new Random().Next(18, 30);
    }
}
