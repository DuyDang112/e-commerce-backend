using Shared.Configurations;

namespace Basket.Api.Services
{
    public class BackgroundJobHttpService
    {
        public HttpClient Client { get; }
        public string ScheduledJobUrl { get; }

        public BackgroundJobHttpService(HttpClient client,
            BackgroundJobSettings jobSettings)
        {
            client.BaseAddress = new Uri(jobSettings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client = client;
            ScheduledJobUrl = jobSettings.ScheduledJobUrl;
        }
    }
}
