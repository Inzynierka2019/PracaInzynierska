namespace Web.Logic.WebUI
{
    using Microsoft.AspNetCore.SignalR;
    using Web.Logic.Hubs;
    using Web.Logic.Services;

    public class WebUIStorage
    {
        private readonly IHubContext<UIHub> hub;
        private readonly ILog Log;

        public WebUIStorage(IHubContext<UIHub> hub, ILog Log)
        {
            this.hub = hub;
            this.Log = Log;
            var communicationManager = new UnityAppCommunicationManager(Log);
        }
    }
}
