namespace Broker.Tests
{
    using System;
    using Broker.Communication;
    using Microsoft.AspNetCore.SignalR;

    public class Program
    {
        static void Main(string[] args)
        {
            //add log4net!
            ICommunicationHub hub = new MessageHub();

            var client1 = new FakeClient(hub);
            var client2 = new FakeClient(hub);

            client1.SendMessage("Hello there, General Kenobi");
        }
    }
}
