using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.SignalR.Client;

namespace Common.HubClient
{
    public abstract class BaseClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseClient));

        #region Public Properties

        /// <summary>
        /// The client's SignalR connection.
        /// </summary>
        public HubConnection Connection { get; set; }

        /// <summary>
        /// The hub's URL address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The name is only necessary for logging purposes.
        /// </summary>
        public string Name { get; set; }

        #endregion

        protected BaseClient(string name = "Client")
        {
            this.Name = name;
            this.Address = "http://localhost:5000/UIHub";
            this.Connection = new HubConnectionBuilder()
                .WithUrl(this.Address)
                .Build();

            this.Connection.Closed += async (error) =>
            {
                await Task.Delay(1000);
                await this.Connection.StartAsync();
            };

            Task.Run(async () => await this.Connect());
            WaitForConnection();
        }

        #region Private Methods
        private async Task Connect()
        {
            try
            {
                await this.Connection.StartAsync();
                Log.Info($"{this.Name} established a connection with {this.Address}");
            }
            catch (Exception ex)
            {
                Log.Error($"{this.Name} could not connect to {this.Address}", ex);
            }
        }

        private void WaitForConnection()
        {
            while (this.Connection.State.Equals(HubConnectionState.Disconnected))
            {
                Thread.Sleep(100);
            }
        }
        #endregion

        #region Public Methods
        public async Task Send<T>(string method, T message)
        {
            try
            {
                await this.Connection.InvokeAsync(method, message);
            }
            catch (Exception ex)
            {
                Log.Error($"Message could not be sent to {this.Address}: ", ex);
            }
        }

        public async Task SendAsync<T>(string method, T message)
        {
            try
            {
                await this.Connection.SendAsync(method, message);
            }
            catch (Exception ex)
            {
                Log.Error($"Message not sent to {this.Address}: ", ex);
            }
        }

        public void Dispose()
        {
            try
            {
                this.Connection.DisposeAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal("Connection could not be disposed!", ex);
            }
        }
        #endregion
    }
}
