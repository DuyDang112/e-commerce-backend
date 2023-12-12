namespace Basket.Api.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateReminderCheckOutOrderEmail(string userName);
    }
}
