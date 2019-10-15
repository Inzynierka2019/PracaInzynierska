namespace Common.Communication.Web
{
    using System;
    using Common.Communication.WebClient;
    using Common.Utils;

    /// <summary>
    /// AppConnector's purpose is to establish a connection with web browser.
    /// </summary>
    public class AppConnector
    {
        private ActionTimer Timer { get; set; }
        private readonly IDebugLogger logger;

        /// <summary>
        /// Timespan in milliseconds before sending first 'keep-alive' message.
        /// </summary>
        // It should be linked with period time of loading unity app!
        private const int dueTime = 2000;

        private readonly TimeSpan interval = new TimeSpan(0, 0, 0, 0, 2000);

        public AppConnector(IDebugLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Starts connection with web app.
        /// </summary>
        public async void StartConnection()
        {
            using (var webClient = new WebClient(this.logger))
            {
                await webClient.Get(Endpoints.Connect);
            }
        }
        /// <summary>
        /// Closes connection with web app.
        /// </summary>
        public async void CloseConnection()
        {
            using (var webClient = new WebClient(this.logger))
            {
                await webClient.Get(Endpoints.Disconnect);
            }
        }
    }
}
