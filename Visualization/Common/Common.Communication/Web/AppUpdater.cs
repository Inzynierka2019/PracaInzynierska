namespace Libraries.Web
{
    using Common.Communication;
    using Common.Communication.WebClient;

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

        #endregion
    }
}
