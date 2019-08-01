namespace Web.Logic.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using log4net;
    using Microsoft.AspNetCore.Hosting;

    public class ProcessService : IProcessService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessService));
        private const string unityAppExecutable = @"out\Simulation.exe";
        private readonly string directoryPath;
        private readonly string unityProjectPath;

        public ProcessService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.directoryPath = this.unityProjectPath = this.hostingEnvironment.ContentRootPath;
            this.unityProjectPath += @"\..\..\Simulation\App";
        }

        public void ExecuteBuildSimulation()
        {
            var cmd = Path.Combine(directoryPath, "Scripts", "build-simulation.bat");
            ExecuteCommand(cmd, true);
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

            process.OutputDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Debug(e.Data);
            process.ErrorDataReceived +=
                (object sender, DataReceivedEventArgs e) => Log.Debug(e.Data);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            SetForegroundWindow(process.Handle);

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
