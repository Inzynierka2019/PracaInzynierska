namespace Broker.Tests
{
    using Broker.Communication;
    using Broker.Models;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// FakeClient class
    /// </summary>
    public class FakeClient
    {
        private readonly ICommunicationHub commHub;

        private long MessageCount { get; set; } = 0; 

        public FakeClient(ICommunicationHub commHub)
        {
            this.commHub = commHub;
        }

        public void SendMessage(string message)
        {
            MessageCount++;
            commHub.SendMessage(new Message { Id = MessageCount, Content = message });
        }
    }
}
