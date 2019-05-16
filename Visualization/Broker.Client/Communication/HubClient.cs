namespace Broker.Communication
{
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

        private readonly string defaultChannel;
        private readonly string hubAddress;

        public HubClient(string addr, string channel)
        {
            this.hubAddress = addr;
            this.defaultChannel = channel;

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
                this.MessagesList.Add("Connection started");
            }
            catch (Exception ex)
            {
                this.MessagesList.Add($"{ex.Message}: {ex}");
            }
        }

        public virtual void DebugInfo()
        {
            foreach(var message in this.MessagesList)
            {
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
        }

        public async Task Send<T>(T message)
        {
            try
            {
                await connection.InvokeAsync(this.defaultChannel, message);
            }
            catch (Exception ex)
            {
                this.MessagesList.Add($"Message not sent to {this.defaultChannel}: {ex}");
            }
        }

        public async Task Send<T>(string channel, T message)
        {
            try
            {
                await connection.InvokeAsync(channel, message);
            }
            catch (Exception ex)
            {
                this.MessagesList.Add($"Message not sent to {channel}: {ex}");
            }
        }

        public async Task SendAsync<T>(T message)
        {
            try
            {
                await connection.SendAsync(this.defaultChannel, message);
            }
            catch (Exception ex)
            {
                this.MessagesList.Add($"Message not sent to {this.defaultChannel}: {ex}");
            }
        }

        public async Task SendAsync<T>(string channel, T message)
        {
            try
            {
                await connection.InvokeAsync(channel, message);
            }
            catch (Exception ex)
            {
                this.MessagesList.Add($"Message not sent to {channel}: {ex}");
            }
        }

        public void Dispose()
        {
            this.connection.DisposeAsync();
        }
    }
}