namespace Broker.Communication
{
    using System;
    using System.Configuration;

    /// <summary>
    /// SignalR client used for debug communication
    /// </summary>
    public class DebugClient : HubClient
    {
        private readonly static string HubName = "debugHub";
        private readonly static string Address = ConfigurationManager.AppSettings["debugHub"];

        public DebugClient() : base(HubName, Address)
        {
        }

        public async override void DebugInfo()
        {
            foreach (var message in this.MessagesList)
            {
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
                await Send(message);
            }
        }
    }
}
