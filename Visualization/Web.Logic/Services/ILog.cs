namespace Web.Logic.Services
{
    using Common.Models.Enums;

    public interface ILog
    {
        void Fatal(object message, LogType type);

        void Error(object message, LogType type);

        void Warn(object message, LogType type);

        void Info(object message, LogType type);

        void Debug(object message, LogType type);
    }
}
