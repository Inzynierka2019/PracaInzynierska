namespace Broker.Communication
{
    using log4net;
    using Microsoft.AspNetCore.SignalR.Client;
    using System.Configuration;

    /// <summary>
    /// SignalR client used for debug communication
    /// </summary>
    public class DebugClient : HubClient
    {
        private readonly static string HubName = "debugHub";
        private readonly static string Address = ConfigurationManager.AppSettings["debugHub"];
        private readonly static string Method = "DebugMessage";
        private readonly ILog log = LogManager.GetLogger(typeof(DebugClient));

        public DebugClient() : base(HubName, Address, Method)
        {
            connection.On<string>(Method, LogMessage);
        }

        private void LogMessage(string message)
        {
            log.Debug(message);
        }
    }
}
