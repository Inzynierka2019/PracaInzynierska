using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.HubClient.HubClient
{
    public interface IDebugLogger
    {
        void Log(object message);

        void LogWarning(object message);

        void LogError(object message);
    }
}
