namespace Common.Models
{
    public class VehiclePopulation
    {
        public string Label = "Vehicle population";
        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Label}: {Count}";
        }
    }
}
