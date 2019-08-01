namespace Web.Logic.WebUI
{
    using Microsoft.AspNetCore.SignalR;
    using Web.Logic.Hubs;

    public class WebUIStorage
    {
        private readonly IHubContext<UIHub> hub;

        public WebUIStorage(IHubContext<UIHub> hub)
        {
            this.hub = hub;
        }
    }
}
