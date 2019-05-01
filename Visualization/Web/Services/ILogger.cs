using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inzynierka.Services
{
    public enum LogType
    {
        NORMAL,
        INFO,
        ERROR,
        WARNING
    }

    public interface ILogger
    {
        void LogInformation(string message);

        void LogDebug(string message);

        void LogError(string message);

        void LogWarning(string message);
    }
}
