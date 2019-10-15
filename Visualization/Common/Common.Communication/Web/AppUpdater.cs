namespace Libraries.Web
{
    using Common.Communication;
    using Common.Communication.WebClient;
    using Common.Models;

    public class AppUpdater : IAppUpdater
    {
        private readonly IDebugLogger logger;
        public WebClient WebClient { get; set; }

        public AppUpdater(IDebugLogger logger, string address)
        {
            this.logger = logger;
            WebClient = new WebClient(logger);
        }

        #region Public Methods
        public void UpdateVehiclePopulationPositions(VehiclePopulation population)
        {
            //To do
        }
        #endregion
    }
}
