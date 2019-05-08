namespace Broker.Communication
{
    using Broker.Models;
    using log4net;
    using Microsoft.AspNetCore.SignalR.Client;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for SignalR clients
    /// </summary>
    public abstract class HubClient : IDisposable
    {
        protected List<string> MessagesList { get; set; } = new List<string>();
        protected void ClearMessages() => MessagesList.Clear();
        protected readonly HubConnection connection;

        private readonly string hubName;
        private readonly string hubAddress;
        private readonly string method;
        private readonly ILog log = LogManager.GetLogger(typeof(HubClient));

        public HubClient(string hubName, string address, string method)
        {
            this.hubName = hubName;
            this.hubAddress = address;
            this.method = method;

            connection = new HubConnectionBuilder()
                .WithUrl(this.hubAddress)
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(1000);
                await connection.StartAsync();
            };

            this.Connect();
        }

        private async void Connect()
        {
            try
            {
                await connection.StartAsync();
                log.Info($"{this.hubName} established a connection: {connection.ToString()}");
            }
            catch (Exception ex)
            {
                log.Error($"{this.hubName} could not connect", ex);
            }
        }

        public async Task Send<T>(T message)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", message);
            }
            catch (Exception ex)
            {
                log.Error($"Message not sent to {this.hubAddress}: {ex}");
            }
        }

        public async Task SendAsync<T>(T message)
        {
            try
            {
                await connection.SendAsync("SendMessage", message);
            }
            catch (Exception ex)
            {
                this.MessagesList.Add($"Message not sent to {this.hubAddress}: {ex}");
            }
        }

        public void Dispose()
        {
            this.connection.DisposeAsync();
        }
    }
}