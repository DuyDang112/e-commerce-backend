using MassTransit;
using ILogger = Serilog.ILogger;

namespace Payment.Api.CourierActivities
{
    public class PaymentActivity : IActivity<PaymentArguments, PaymentLog>
    {
        private readonly ILogger _logger;

        public PaymentActivity(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<PaymentArguments> context)
        {
            if (context.Arguments.CardNumber.StartsWith("5999"))
                throw new InvalidOperationException("The card number was invalid");

            var paymentId = Guid.NewGuid();
            _logger.Information($"------- Executed Payment OrderId: {context.Arguments.OrderId}-----------------");
            return context.Completed(new {PaymentId = paymentId});
        }

        public async Task<CompensationResult> Compensate(CompensateContext<PaymentLog> context)
        {
            _logger.Information($"--------Compensated PaymentId: {context.Log.PaymentId}------------");

            return context.Compensated();
        }

    }
}
