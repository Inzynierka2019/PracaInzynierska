using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Libraries.Web
{
    public interface IAppUpdater
    {
        HubClient HubClient { get; set; }
    }
}
