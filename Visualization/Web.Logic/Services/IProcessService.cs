using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Logic.Services
{
    public interface IProcessService
    {
        void ExecuteBuildSimulation();

        void ExecuteRunSimulation();
    }
}
