namespace Common.Communication
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.HubClient.HubClient;
    using Microsoft.AspNetCore.SignalR.Client;

    public abstract class BaseClient : IDisposable
    {
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

        /// <summary>
        /// The Logger.
        /// </summary>
        protected IDebugLogger Logger { get; set; }

        protected BaseClient(string name, string address, IDebugLogger Logger)
        {
            this.Logger = Logger;
            this.Name = name;
            this.Address = address;

            this.Connection = new HubConnectionBuilder()
                .WithUrl(this.Address)
                .Build();

            this.Connection.Closed += (error) =>
            {
                this.Connection.StartAsync();
                return Task.Delay(1000);
            };

            this.Connect();
            WaitForConnection();
        }

        #region Private Methods
        private void Connect()
        {
            try
            {
                this.Connection.StartAsync();
                Logger.Log($"{this.Name} established a connection with {this.Address}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"{this.Name} could not connect to {this.Address}: {ex}");
            }
        }

        private void WaitForConnection()
        {
            while (this.Connection.State.Equals(HubConnectionState.Disconnected))
            {
                Logger.LogWarning($"Waiting for {this.Name} to connect with SignalR!");
            }
        }
        #endregion

        #region Public Methods
        public void Send<T>(string method, T message)
        {
            try
            {
                this.Connection.InvokeAsync(method, message);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Message could not be sent to {this.Address}: {ex}");
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
                Logger.LogError($"Connection could not be disposed! {ex}");
            }
        }
        #endregion
    }
}
