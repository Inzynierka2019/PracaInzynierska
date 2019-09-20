namespace Web.Logic.WebUI
{
    using System;
    using Microsoft.AspNetCore.SignalR;

    using Common.Models.Enums;
    using Common.Communication;
    using Web.Logic.Services;
    using Web.Logic.Hubs;
    using System.Threading.Tasks;

    public class UnityAppStatusUpdater
    {
        private readonly IHubContext<UIHub> hub;
        private readonly ILog Log;
        private readonly string status = SignalMethods.SignalForUnityAppConnectionStatus.Method;
        private readonly UnityAppCommunicationManager communicationManager;

        public UnityAppStatusUpdater(IHubContext<UIHub> hub, ILog log, UnityAppCommunicationManager communicationManager)
        {
            this.hub = hub;
            this.Log = log;
            this.communicationManager = communicationManager;
        }

        public async Task UnityAppConnectionStatus(bool isConnected)
        {
            try
            {
                //if (isConnected)
                //    this.communicationManager.Connected();

                await Task.Run(() =>
                {
                    return this.hub.Clients.All.SendAsync(status, isConnected);
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Exception was thrown while sending Unity App connection status {ex.Message}");
            }
        }
    }
}
