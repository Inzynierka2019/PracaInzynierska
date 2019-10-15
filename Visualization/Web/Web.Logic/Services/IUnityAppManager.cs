using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Logic.Services
{
    public interface IUnityAppManager
    {
        void CheckStatus(bool isConnected);
        TimeSpan GetAppTimeSpan();
    }
}
