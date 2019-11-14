namespace Common.Models
{
    using System;

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
            this.ReactionTime = 0.5f;
        }
    }
}
