using Infrastructures.Extentions;
using Shared.Configurations;
using Shared.DTOs.ScheduledJob;

namespace Basket.Api.Services
{
    public class BackgroundJobHttpService
    {
        private readonly HttpClient _client;
        private readonly string _scheduledJobUrl;

        public BackgroundJobHttpService(HttpClient client,
            BackgroundJobSettings jobSettings)
        {
            client.BaseAddress = new Uri(jobSettings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
            _scheduledJobUrl = jobSettings.ScheduledJobUrl;
        }

        public async Task<string> SendEmailReminderCheckoutOrder(ReminderCheckoutOrderDto model)
        {
            var uri = $"{_scheduledJobUrl}/send-email-reminder-checkout-order";
            var response = await _client.PostAsJson(uri, model);
            string jobId = null;
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                jobId = await response.ReadContentAs<string>();
            }

            return jobId;
        }

        public void DeleteReminderCheckoutOrder(string jobId)
        {
            var url = $"{_scheduledJobUrl}/delete/jobId/{jobId}";
            _client.DeleteAsync(url);
        }
    }
}
