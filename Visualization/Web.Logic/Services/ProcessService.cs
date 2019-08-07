namespace Web.Logic.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    using Common.Models.Enums;
    using Web.Logic.Configuration;

    public class ProcessService : IProcessService
    {
        private readonly IUnityConfiguration unityConfig;
        private readonly ILog Log;
        private readonly string directoryPath;

        public ProcessService(IHostingEnvironment hostingEnvironment, IUnityConfiguration unityConfig, ILog log)
        {
            this.directoryPath = hostingEnvironment.ContentRootPath;
            this.unityConfig = unityConfig;
            this.Log = log;
        }

        public bool ExecuteBuildSimulation()
        {
            var path = Path.Combine(directoryPath, "Scripts", "build-simulation.bat");
            var unityAppDir = Path.Combine(this.directoryPath, this.unityConfig.ProjectPath);
            var cmd = string.Join(" ", path, this.unityConfig.UnityExe, unityAppDir);

            return ExecuteCommand(cmd, true);
        }

        public void ExecuteRunSimulation()
        {
            var cmd = Path.Combine(directoryPath, this.unityConfig.OutputDirectory, this.unityConfig.UnityAppExe);
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
