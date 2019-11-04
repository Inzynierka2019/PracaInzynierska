using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Models
{

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), Title = "scenes")]
    public class SimulationPreferences
    {
        public IList<ScenePreferences> ScenePreferences { get; set; }
    }
}
