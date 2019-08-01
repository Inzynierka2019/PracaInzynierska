namespace Web.Logic.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    using Common.Models.Enums;

    public class ProcessService : IProcessService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILog Log;
        private const string unityAppExecutable = @"out\Simulation.exe";
        private readonly string directoryPath;
        private readonly string unityProjectPath;

        public ProcessService(IHostingEnvironment hostingEnvironment, ILog log)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.Log = log;
            this.directoryPath = this.unityProjectPath = this.hostingEnvironment.ContentRootPath;
            this.unityProjectPath += @"\..\..\Simulation\App";
        }

        public void ExecuteBuildSimulation()
        {
            var cmd = Path.Combine(directoryPath, "Scripts", "build-simulation.bat");
            if(ExecuteCommand(cmd, true))
            {
                Log.Info("Build has successfully finished!", LogType.Success);
            }
        }

        public void ExecuteRunSimulation()
        {
            ExecuteCommand(unityAppExecutable, false);
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
            processInfo.WorkingDirectory = unityProjectPath;
            processInfo.CreateNoWindow = true;

            var process = Process.Start(processInfo);
            SetForegroundWindow(process.Handle);

            process.OutputDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Info(e.Data, LogType.Info);
            process.ErrorDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Info(e.Data, LogType.Info);

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
    }
}
