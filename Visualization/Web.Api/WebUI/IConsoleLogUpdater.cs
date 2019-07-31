namespace Web.Api.WebUI
{
    using System.Threading.Tasks;

    using Common.Models.Enums;

    public interface IConsoleLogUpdater
    {
        Task SendConsoleLog(string message, LogMessageType type);
    }
}
