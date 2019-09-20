namespace Web.Logic.Services
{
    public class UnityAppCommunicationManager
    {
        public bool IsConnectedWithApp { get; set; } = false;

        private readonly ILog Log;

        public UnityAppCommunicationManager(ILog log)
        {
            Log = log;
            Log.Success("Unity App Communication Manager is now running...");
        }

        public void Connected()
        {
            IsConnectedWithApp = true;
            Log.Success("Unity App is now connected!");
        }

        public void Disconnected()
        {
            IsConnectedWithApp = true;
            Log.Warn("Unity App has disconnected.");
        }
    }
}
