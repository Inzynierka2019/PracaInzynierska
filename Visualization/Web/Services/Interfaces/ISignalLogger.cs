using Microsoft.AspNetCore.SignalR;

namespace Web.Services
{
    public enum LogType
    {
        NORMAL,
        INFO,
        ERROR,
        WARNING
    }

    public interface ISignalLogger<T> where T : Hub
    {
        void Info(string message);

        void Debug(string message);

        void Error(string message);

        void Warning(string message);
    }
}
