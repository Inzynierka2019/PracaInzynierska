namespace Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum Personality
    {
        Slow,
        Normal,
        Aggresive,
    }

    public class Driver
    {
        public Personality Personality { get; set; }
        public float ReactionTime { get; set; }
        public float Age { get; set; }
    }

    public static class DriverFactory
    {
        public static List<DriverSpawnChance> driverSpawnChances
            = new List<DriverSpawnChance>();
        public static Driver CreateDriver(Personality personality)
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

        public static Driver GetRandomDriver()
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

    public class SlowDriver : Driver
    {
        public SlowDriver()
        {
            this.Personality = Personality.Slow;
            this.Age = new Random().Next(50, 80);
            this.ReactionTime = 0.2f;
        }
    }

    public class NormalDriver : Driver
    {
        public NormalDriver() : base()
        {
            this.Personality = Personality.Normal;
            this.Age = new Random().Next(25, 60);
            this.ReactionTime = 0.1f;
        }
    }

    public class AggresiveDriver : Driver
    {
        public AggresiveDriver()
        {
            this.Personality = Personality.Aggresive;
            this.Age = new Random().Next(18, 30);
            this.ReactionTime = 0.05f;
        }
    }
}
