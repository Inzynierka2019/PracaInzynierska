using log4net;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Hubs;

namespace Web.Api.WebUI
{
    public class WebUIStorage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebUIStorage));

        private readonly IHubContext<UIHub> hubContext;

        public WebUIStorage()
        {
        }
    }
}
