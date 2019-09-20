namespace Common.Communication
{
    using System;
    using Common.Utils;

    /// <summary>
    /// AppConnector's purpose is to establish a connection with web browser.
    /// </summary>
    public class AppConnector : IDisposable
    {
        private ActionTimer Timer { get; set; }
        private HubClient HubClient { get; set; }
            = new HubClient(SignalMethods.SignalForUnityAppConnectionStatus, "KeepAlive");

        /// <summary>
        /// Timespan in milliseconds before sending first 'keep-alive' message.
        /// </summary>
        private const int dueTime = 2000;

        private readonly TimeSpan interval = new TimeSpan(0, 0, 5);

        private readonly string keepAlive = SignalMethods.SignalForUnityAppConnectionStatus.Method;

        public AppConnector() { }

        /// <summary>
        /// Sends 'keep-alive' message to web browser with a specified interval.
        /// </summary>
        public void KeepAlive()
        {
            Timer = new ActionTimer(
              async () => await this.HubClient.Send(keepAlive, true),
              interval,
              dueTime);
        }

        public async void Disconnect()
        {
            Timer.Dispose();
            await this.HubClient.Send(keepAlive, false);
        }

        public void Dispose()
        {
            Disconnect();
            ((IDisposable)HubClient).Dispose();
        }
    }
}
