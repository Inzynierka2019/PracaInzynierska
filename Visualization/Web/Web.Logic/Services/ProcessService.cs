﻿namespace Web.Logic.Services
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
            var cmd = Path.Combine(directoryPath, AppDomain.CurrentDomain.BaseDirectory, unityAppExe);
            ExecuteCommand(cmd, false);
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        private bool ExecuteCommand(string command, bool waitForExit)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);
            //SetForegroundWindow(process.Handle);

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
