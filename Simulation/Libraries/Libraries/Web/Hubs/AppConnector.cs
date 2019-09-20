namespace Libraries.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using Common.HubClient;

    public class AppConnector : IDisposable
    {
        public bool _threadRunning { get; set; }
        private HubClient HubClient { get; set; }
        private ActionTimer timer { get; set; }

        private List<SignalMethod> GetKeepAliveMethod()
        {
            return new List<SignalMethod>
            {
                SignalMethods.SignalForUnityAppConnectionStatus
            };
        }

        public AppConnector()
        {
            this.HubClient = new HubClient(GetKeepAliveMethod(), "KeepAlive");
        }

        public void KeepAliveMessage()
        {
            this.timer = new ActionTimer(
              async () => await this.HubClient.Send(
                  SignalMethods.SignalForUnityAppConnectionStatus.Method,
                  true),
              new TimeSpan(0, 0, 5));
        }

        public void Dispose()
        {
            ((IDisposable)timer).Dispose();
            ((IDisposable)HubClient).Dispose();
        }
    }
}
