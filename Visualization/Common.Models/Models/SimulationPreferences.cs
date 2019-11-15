using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Models
{
    public class VehicleSpawnChance
    {
        public string routeType { get; set; }
        public int spawnChance { get; set; }
    }

    public class ScenePreference
    {
        public float vehicleSpawnFrequency { get; set; }
        public int slowDriverSpawnChance { get; set; }
        public int normalDriverSpawnChance { get; set; }
        public int aggresiveDriverSpawnChance { get; set; }
        public int vehicleCountMaximum { get; set; }
        public int trafficLightsPeriod { get; set; }
        public List<VehicleSpawnChance> vehicleSpawnChances { get; set; }
    }

    public class SimulationPreferences
    {
        public string currentScene { get; set; }
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
