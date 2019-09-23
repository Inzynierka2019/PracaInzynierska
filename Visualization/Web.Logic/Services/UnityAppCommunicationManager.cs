namespace Web.Logic.Services
{
    public class UnityAppCommunicationManager
    {
        public bool IsConnectedWithApp { get; set; } = false;

        private readonly ILog Log;

        public UnityAppCommunicationManager(ILog log)
        {
            Log = log;
            Log.Success("Unity App Communication Manager is now running.");
        }

        public void Connected(bool keepAlive)
        {
            if (!keepAlive)
            {
                IsConnectedWithApp = true;
                Log.Success("Simulation App is now connected!");
            }
            else // app is already connected here.
            {
            }
        }

        public void Disconnected()
        {
            Log.Warn("Simulation App has disconnected.");
        }

        public void CheckStatus(bool status)
        {
            if (status)
                Connected(status == IsConnectedWithApp);
            else Disconnected();
        }
    }
}