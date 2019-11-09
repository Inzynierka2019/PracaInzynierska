using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Logic.Services
{
    public interface IProcessService
    {
        void ExecuteRunSimulationWindows();
        SimulationPreferences GetJsonSimulationPreferences();
        void SaveJsonSimulationPreferences(SimulationPreferences preferences);
    }
}
