using Common.Models;

namespace Common.Models
{
    public class VehicleStatus
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float CurrentSpeed { get; set; }
        public Personality Personality { get; set; }
    }
}
