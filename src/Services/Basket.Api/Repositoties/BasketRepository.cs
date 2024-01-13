using Basket.Api.Entities;
using Basket.Api.Repositoties.Interface;
using Basket.Api.Services;
using Basket.Api.Services.Interfaces;
using Contract.Common.Interfaces;
using Infrastructures.Extentions;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTOs.ScheduledJob;
using ILogger = Serilog.ILogger;

namespace Basket.Api.Repositoties
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;
        private readonly BackgroundJobHttpService _backgroundJobHttp;
        private readonly IEmailTemplateService _emailTemplateService;
        public BasketRepository(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger,
            BackgroundJobHttpService backgroundJobHttp,
            IEmailTemplateService emailTemplateService)
        {
            _redisCacheService = redisCacheService;
            _serializeService = serializeService;
            _logger = logger;
            _backgroundJobHttp = backgroundJobHttp;
            _emailTemplateService = emailTemplateService;
        }

        public async Task<bool> DeleteBasketFromUserName(string userName)
        {
            DeleteReminderCheckoutOrder(userName);
            try
            {
                _logger.Information($"BEGIN: DeleteBasketFromUserName {userName}");
                await _redisCacheService.RemoveAsync(userName);
                _logger.Information($"END: DeleteBasketFromUserName {userName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"DeleteBasketFromUserName {ex.Message}");
                throw;
            }
        }

        public async Task<Cart> GetBasketByUserName(string userName)
        {
            _logger.Information($"BEGIN: GetBasketByUserName {userName}");
            var basket = await _redisCacheService.GetStringAsync(userName);
            _logger.Information($"END: GetBasketByUserName {userName}");
            return string.IsNullOrEmpty(basket) ? null :
                _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            DeleteReminderCheckoutOrder(cart.UserName);

            _logger.Information($"BEGIN: UpdateBasket {cart.UserName}");
            if (options != null)
            {
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart),options);
            }
            else
            {
                await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
            }
            _logger.Information($"END: UpdateBasket {cart.UserName}");

            try
            {
                await TriggerSendEmailReminderCheckoutOrder(cart);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return await GetBasketByUserName(cart.UserName);
        }

        private async Task TriggerSendEmailReminderCheckoutOrder(Cart cart)
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckOutOrderEmail(cart.UserName);
            var model = new ReminderCheckoutOrderDto(cart.EmailAddress, "ReminderCheckout", 
                emailTemplate,DateTimeOffset.UtcNow.AddSeconds(30));

            var jobId = await _backgroundJobHttp.SendEmailReminderCheckoutOrder(model);

            if(!string.IsNullOrEmpty(jobId))
            {
                cart.JobId = jobId;
                await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
            }
        }

        private async Task DeleteReminderCheckoutOrder(string username)
        {
            var cart = await GetBasketByUserName(username);
            if (cart == null || string.IsNullOrEmpty(cart.JobId)) return;

            var jobId = cart.JobId;
           _backgroundJobHttp.DeleteReminderCheckoutOrder(jobId);
            _logger.Information($"DeleteReminderCheckoutOrder:Delete JobId {jobId}");
        }
    }
}
