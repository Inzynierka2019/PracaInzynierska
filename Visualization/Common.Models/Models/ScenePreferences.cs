using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ScenePreferences
    {
        public string SceneName { get; set; }

        public int InitialVehiclePopulation { get; set; }

        public string SceneImageLocation { get; set; }
    }
}
