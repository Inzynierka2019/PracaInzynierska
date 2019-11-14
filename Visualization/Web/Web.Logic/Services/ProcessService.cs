namespace Web.Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Common.Models;

    public class ProcessService : IProcessService
    {
        private readonly ILog Log;
        private readonly string directoryPath;
        private readonly IConfiguration config;

        public ProcessService(IHostingEnvironment hostingEnvironment, ILog log, IConfiguration configuration)
        {
            this.directoryPath = hostingEnvironment.ContentRootPath;
            this.Log = log;
            this.config = configuration;
        }

        public void ExecuteRunSimulationWindows()
        {
            var unityAppExe = this.config["UnityAppExe"];
            var unityAppDirectory = this.config["UnityAppDirectory"];
            var workingDirectory = Path.Combine(directoryPath, AppDomain.CurrentDomain.BaseDirectory, unityAppDirectory);
            var command = Path.Combine(workingDirectory, unityAppExe);
            ExecuteCommand(command, workingDirectory, false);
        }

        private bool ExecuteCommand(string command, string workingDirectory, bool waitForExit)
        {
            this.Log.Info($"Working directory: {workingDirectory}");

            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = workingDirectory;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            var process = Process.Start(processInfo);

            process.OutputDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Info(e.Data);
            process.ErrorDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Info(e.Data);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (waitForExit)
            {
                process.WaitForExit();
                var exitCode = process.ExitCode;
                process.Close();
                return exitCode == 0;
            }

            return true;
        }

        public void SaveJsonSimulationPreferences(SimulationPreferences preferences)
        {
            var workingDir = Directory.GetCurrentDirectory();
            var configName = this.config["SimulationPreferences"];

            File.WriteAllText(
                Path.Combine(workingDir, configName), 
                JsonConvert.SerializeObject(preferences, Formatting.Indented));
        }


        public SimulationPreferences GetJsonSimulationPreferences()
        {
            var workingDir = Directory.GetCurrentDirectory();
            var configName = this.config["SimulationPreferences"];

            using (var reader = new StreamReader(Path.Combine(workingDir, configName)))
            {
                var json = reader.ReadToEnd();
                var simulationPreferences = JsonConvert.DeserializeObject<SimulationPreferences>(json);

                return simulationPreferences;
            }
        }
    }
}
