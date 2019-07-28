using log4net;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Web.Logic.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessService));
        private readonly string directoryPath;

        public ProcessService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.directoryPath = this.hostingEnvironment.ContentRootPath;
        }

        public void ExecuteBuildSimulation()
        {
            var cmd = Path.Combine(directoryPath, "Scripts", "build-simulation.bat");
            ExecuteCommand(cmd);
        }

        public void ExecuteRunSimulation()
        {
            var cmd = Path.Combine(directoryPath, "Scripts", "run-simulation.bat");
            ExecuteCommand(cmd);
        }

        private bool ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = System.Diagnostics.Process.Start(processInfo);

            process.OutputDataReceived += 
                (object sender, DataReceivedEventArgs e) => Log.Debug(e.Data);
            process.ErrorDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Debug(e.Data);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            var exitCode= process.ExitCode;
            process.Close();

            return exitCode == 0;
        }
    }
}
