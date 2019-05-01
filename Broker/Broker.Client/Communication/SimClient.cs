namespace Broker.Communication
{
    using System.Configuration;
    
    /// <summary>
    /// SignalR client used for simulation-related communication
    /// </summary>
    public class SimClient : HubClient
    {
        private readonly static string HubName = "simulationHub";
        private readonly static string Address = ConfigurationManager.AppSettings[HubName];

        public SimClient() : base(HubName, Address)
        {
        }
    }
}
