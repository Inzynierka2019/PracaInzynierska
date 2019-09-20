namespace Web.Logic.Services
{
    using Common.Models.Enums;

    public interface ILog
    {
        void Fatal(object message);

        void Error(object message);

        void Warn(object message);

        void Info(object message);

        void Debug(object message);

        void Success(object message);
    }
}
