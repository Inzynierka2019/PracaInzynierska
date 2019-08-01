namespace Web.Logic.WebUI
{
    using System.Threading.Tasks;

    using Common.Models.Enums;

    public interface IConsoleLogUpdater
    {
        Task SendConsoleLog(string message, LogType type);
    }
}
