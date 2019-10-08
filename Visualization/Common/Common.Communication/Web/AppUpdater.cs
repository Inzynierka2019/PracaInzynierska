using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.HubClient.HubClient;

namespace Libraries.Web
{
    public class AppUpdater : IAppUpdater
    {
        private readonly IDebugLogger logger;
        public HubClient HubClient { get; set; }

        public AppUpdater(IDebugLogger logger)
        {
            this.logger = logger;
            HubClient = new HubClient(logger, "AppUpdater");
        }

        #region Public Methods

        #endregion
    }
}
