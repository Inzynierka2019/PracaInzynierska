namespace Common.Communication.WebClient
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class WebClient : IDisposable
    {
        private readonly IDebugLogger logger;
        private HttpClient HttpClient { get; set; }

        private HttpContent Serialize(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }

        public WebClient(IDebugLogger logger)
        {
            this.logger = logger;
            this.HttpClient = new HttpClient();
        }

        public async Task Get(string url)
        {
            try
            {
                await this.HttpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
            }
        }

        public async Task Post(string url, object data)
        {
            try
            {
                await this.HttpClient.PostAsync(url, this.Serialize(data));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
            }
        }

        public async Task Put(string url, object data)
        {
            try
            {
                await this.HttpClient.PutAsync(url, this.Serialize(data));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
            }
        }

        public void Dispose()
        {
            this.HttpClient.Dispose();
        }
    }
}