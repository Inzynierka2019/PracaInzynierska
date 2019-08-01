namespace Web.Logic.WebUI
{
    using System;
    using Microsoft.AspNetCore.SignalR;

    using Web.Logic.Services;
    using Web.Logic.Hubs;
    using Common.Models.Enums;

    public class UnityAppStatusUpdater
    {
        private readonly IHubContext<UIHub> hub;
        private readonly ILog Log;

        public UnityAppStatusUpdater(IHubContext<UIHub> hub, ILog log)
        {
            this.hub = hub;
            this.Log = log;
        }

        public void Send(bool isConnected)
        {
            try
            {
               // this.hub.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus, isConnected);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending Unity App connection status {ex.Message}", LogType.Error);
            }
        }
    }
}
