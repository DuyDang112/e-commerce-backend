using Basket.Api.Services.Interfaces;
using Shared.Configurations;

namespace Basket.Api.Services
{
    public class BasketEmaiTemplateService : EmailTemplateService, IEmailTemplateService
    {
        public BasketEmaiTemplateService(BackgroundJobSettings jobSettings) : base(jobSettings)
        {
        }

        public string GenerateReminderCheckOutOrderEmail(string userName)
        {
            var _checkoutUrl = $"{_jobSettings.ApigwUrl}/{_jobSettings.BasketUrl}/{userName}";
            var emailText = ReadEmailTemplateContent("reminder-checkout-order");
            var emailReplaceTextString = emailText.Replace("[UserName]", userName)
                .Replace("[CheckoutUrl]", _checkoutUrl);

            return emailReplaceTextString;
        }

    }
}
