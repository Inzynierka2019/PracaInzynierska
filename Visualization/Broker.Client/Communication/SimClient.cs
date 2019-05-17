namespace Broker.Communication
{
    using Broker.Models;
    using log4net;
    using System.Configuration;
    using Microsoft.AspNetCore.SignalR.Client;

    /// <summary>
    /// SignalR client used for simulation-related communication
    /// </summary>
    public class SimClient : HubClient
    {
        private static readonly string HubName = "simulationHub";
        private static readonly string Address = ConfigurationManager.AppSettings[HubName];
        private readonly static string Method = "SimulationMessage";
        private readonly ILog log = LogManager.GetLogger(typeof(SimClient));

        public SimClient() : base(HubName, Address, Method)
        {
            connection.On<Message>(Method, LogMessage);
        }

        private void LogMessage(Message message)
        {
            log.Debug(message);
        }
    }
}
