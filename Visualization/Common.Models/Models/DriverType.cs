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
