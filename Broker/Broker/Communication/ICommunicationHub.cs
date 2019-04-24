namespace Broker.Communication
{
    using Broker.Models;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    /// <summary>
    /// ICommunicationHub interface
    /// </summary>
    /// <remarks>
    /// Main interface for communication using SignalR
    /// </remarks>
    public interface ICommunicationHub
    {
        Task ThrowException();

        Task SendMessage(IMessage message);
    }
}
