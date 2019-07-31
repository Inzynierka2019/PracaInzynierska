using Common.HubClient;
using log4net;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Hubs;

namespace Web.Api.WebUI
{
    public class UnityAppStatusUpdater
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UnityAppStatusUpdater));

        private readonly IHubContext<UIHub> hub;

        public UnityAppStatusUpdater(IHubContext<UIHub> hub)
        {
            this.hub = hub;
        }

        public void Send(bool isConnected)
        {
            try
            {
               // this.hub.Clients.All.SendAsync(SignalMethods.SignalForUnityAppConnectionStatus, isConnected);
            }
            catch (Exception ex)
            {
                Log.Error("Exception was thrown while sending Unity App connection status", ex);
            }
        }
    }
}
