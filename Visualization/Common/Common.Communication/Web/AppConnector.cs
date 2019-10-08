namespace Common.Communication
{
    using System;
    using Common.HubClient.HubClient;
    using Common.Utils;

    /// <summary>
    /// AppConnector's purpose is to establish a connection with web browser.
    /// </summary>
    public class AppConnector : IDisposable
    {
        private ActionTimer Timer { get; set; }
        private HubClient HubClient { get; set; }
        private readonly IDebugLogger logger;

        /// <summary>
        /// Timespan in milliseconds before sending first 'keep-alive' message.
        /// </summary>
        // It should be linked with period time of loading unity app!
        private const int dueTime = 500;

        private readonly TimeSpan interval = new TimeSpan(0, 0, 0, 0, 2000);

        private readonly string keepAlive = SignalMethods.SignalForUnityAppConnectionStatus.Method;

        public AppConnector(IDebugLogger logger)
        {
            this.logger = logger;
            HubClient = new HubClient(SignalMethods.SignalForUnityAppConnectionStatus, logger, "KeepAlive");
        }

        /// <summary>
        /// Sends 'keep-alive' message to web browser with a specified interval.
        /// </summary>
        public void KeepAlive()
        {
            Timer = new ActionTimer(
              async () =>
              {
                  logger.Log("Sending keep-alive");
                  await this.HubClient.Send(keepAlive, true);
              },
              interval,
              dueTime);
        }

        public async void Disconnect()
        {
            await this.HubClient.Send(keepAlive, false);
        }

        public void Dispose()
        {
            Disconnect();
            ((IDisposable)Timer).Dispose();
            ((IDisposable)HubClient).Dispose();
        }
    }
}
