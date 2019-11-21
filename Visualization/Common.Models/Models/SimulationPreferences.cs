using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Models
{
    public class VehicleSpawnChance
    {
        public string routeType { get; set; }
        public int spawnChance { get; set; }
    }

    public class DriverSpawnChance
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Personality personality { get; set; }
        public int spawnChance { get; set; }
    }

    public class ScenePreference
    {
        public List<DriverSpawnChance> driverSpawnChances { get; set; }
        public float trafficLightsPeriod { get; set; }
        public float vehicleSpawnFrequency { get; set; }
        public int vehicleCountMaximum { get; set; }
        public List<VehicleSpawnChance> vehicleSpawnChances { get; set; }
    }

    public class SimulationPreferences
    {
        public string currentScene { get; set; }
        public List<string> availableScenes { get; set; }
        public ScenePreference scenePreferences { get; set; }
    }
}
