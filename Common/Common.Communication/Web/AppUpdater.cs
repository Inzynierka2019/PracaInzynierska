using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Libraries.Web
{
    public class AppUpdater : IAppUpdater
    {
        public HubClient HubClient { get; set; } = new HubClient();

        public AppUpdater() { }

        #region Public Methods
        
        #endregion
    }
}
