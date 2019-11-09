using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Models
{
    public class VehicleSpawnChance
    {
        public string routeType { get; set; }
        public string spawnChance { get; set; }
    }

    public class ScenePreference
    {
        public float vehicleSpawnFrequency { get; set; }
        public string vehicleCountMaximum { get; set; }
        public List<VehicleSpawnChance> vehicleSpawnChances { get; set; }
    }

    public class SimulationPreferences
    {
        public string currentScene { get; set; }
        public List<string> availableScenes { get; set; }
        public ScenePreference scenePreferences { get; set; }
    }


    //[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "scenes")]
}
