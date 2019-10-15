namespace Libraries.Web
{
    using Common.Communication.WebClient;

    public interface IAppUpdater
    {
        WebClient WebClient { get; set; }
    }
}
