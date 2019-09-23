using Libraries.Web;
using System;
using System.Collections.Generic;
using System.Text;
using Common.HubClient;

namespace Libraries.Tests.Mocks
{
    public class FakeAppUpdater : IAppUpdater
    {
        public HubClient HubClient { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
