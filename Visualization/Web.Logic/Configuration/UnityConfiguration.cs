using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Logic.Configuration
{
    public class UnityConfiguration : IUnityConfiguration
    {
        public string UnityExe { get; set; }

        public string UnityAppExe { get; set; }

        public string ProjectPath { get; set; }

        public string BuildSimulationBatch { get; set; }

        public string RunSimulationBatch { get; set; }
    }
}
