using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Logic.Configuration
{
    public interface IUnityConfiguration
    {
        string UnityExe { get; set; }

        string UnityAppExe { get; set; }

        string ProjectPath { get; set; }

        string BuildSimulationBatch { get; set; }

        string RunSimulationBatch { get; set; }
    }
}
