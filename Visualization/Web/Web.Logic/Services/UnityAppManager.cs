namespace Web.Logic.Services
{
    using Common.Models;
    using Common.Models.Enums;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using Web.Logic.Hubs;
    using Web.Logic.Utils;

    public class UnityAppManager : IUnityAppManager
    {
        public DateTime AppStart { get; set; }
        public TimeSpan AppTimeSpan { get; set; }
        public UnityAppState AppState { get; set; }

        private readonly ILog Log;
        private readonly IHubContext<UIHub> hub;
        private readonly IStatisticsService statisticsService;
        private ActionTimer StateUpdater;

        public UnityAppManager(ILog Log, IHubContext<UIHub> hub, IStatisticsService statisticsService)
        {
            this.Log = Log;
            this.hub = hub;
            this.statisticsService = statisticsService;
            AppTimeSpan = new TimeSpan();
            AppState = UnityAppState.NOT_CONNECTED;
            Log.Success("Unity App Communication Manager is now running.");
        }

        public void Connected()
        {
            AppStart = DateTime.Now;
            SendAppState(UnityAppState.CONNECTED);
            AppState = UnityAppState.RUNNING;
            Log.Success("Simulation App is now connected!");
            statisticsService.InitializeStatisticsService();
            StateUpdater = new ActionTimer(() => SendAppState(), new TimeSpan(0, 0, 5));
        }

        public void Disconnected()
        {
            AppState = UnityAppState.DISCONNECTED;
            AppTimeSpan = DateTime.Now.Subtract(AppStart);
            SendAppState();
            StateUpdater.Dispose();
            Log.Warn("Simulation App has disconnected.");
        }

        public void UpdateState(UnityAppState status)
        {
            switch (status)
            {
                case UnityAppState.CONNECTED:
                    this.Connected();
                    break;
                case UnityAppState.DISCONNECTED:
                    this.Disconnected();
                    break;
            }
        }

        public TimeSpan GetTimeSpan()
        {
            return this.AppTimeSpan;
        }

        public void SendAppState()
        {
            this.hub.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, this.AppState);
        }

        public void SendAppState(UnityAppState state)
        {
            this.hub.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus.Method, state);
        }
    }
}