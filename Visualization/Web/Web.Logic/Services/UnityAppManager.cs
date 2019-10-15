using System;

namespace Web.Logic.Services
{
    public class UnityAppManager : IUnityAppManager
    {
        public bool IsConnectedWithApp { get; set; } = false;
        public DateTime AppStart { get; set; }
        public TimeSpan AppTimeSpan { get; set; }

        private readonly ILog Log;

        public UnityAppManager(ILog log)
        {
            Log = log;
            Log.Success("Unity App Communication Manager is now running.");
        }

        public void Connected(bool keepAlive)
        {
            if (!IsConnectedWithApp)
            {
                AppStart = DateTime.Now;
                AppTimeSpan = new TimeSpan();
                IsConnectedWithApp = true;
                Log.Success("Simulation App is now connected!");
            }
            else // app is already connected here.
            {
            }
        }

        public void Disconnected()
        {
            AppTimeSpan = DateTime.Now.Subtract(AppStart);
            Log.Warn("Simulation App has disconnected.");
        }

        public void CheckStatus(bool status)
        {
            if (status)
                Connected(status == IsConnectedWithApp);
            else Disconnected();
        }

        public TimeSpan GetAppTimeSpan()
        {
            return this.AppTimeSpan;
        }
    }
}