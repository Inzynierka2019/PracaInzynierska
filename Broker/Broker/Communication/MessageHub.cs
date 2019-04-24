namespace Broker.Communication
{
    using System;
    using System.Threading.Tasks;
    using Broker.Exceptions;
    using Broker.Models;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Example Hub for communication
    /// </summary>
    public class MessageHub : Hub, ICommunicationHub
    {
        public string Connection { get; set; }

        public MessageHub() : base()
        {
            Connection = "DefaultConnection";
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public Task SendMessage(IMessage message)
        {
            return Clients.All.SendAsync(this.Connection, message);
        }

        public Task ThrowException()
        {
            throw new CustomException();
        }
    }
}
