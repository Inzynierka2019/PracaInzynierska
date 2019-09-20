using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.HubClient;

namespace Libraries.Web
{
    public interface IAppUpdater
    {
        HubClient HubClient { get; set; }
    }
}
