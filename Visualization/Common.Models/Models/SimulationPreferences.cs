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
        public string currentSceneName { get; set; }
        public List<SceneData> availableScenes { get; set; }
        public ScenePreference scenePreferences { get; set; }
    }

    public class SceneData
    {
        public string scene { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
