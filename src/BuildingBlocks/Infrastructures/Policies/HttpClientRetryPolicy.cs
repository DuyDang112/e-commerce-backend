using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Policies
{
    public static class HttpClientRetryPolicy
    {
        public static IHttpClientBuilder UseImmediateHttpRetryPolicy(this IHttpClientBuilder builder, int retryCount = 3)
        {
            return builder.AddPolicyHandler(ConfigureImmediateHttpRetry(retryCount));
        }

        public static IHttpClientBuilder UseLinearHttpRetryPolicy(this IHttpClientBuilder builder, int retryCount = 3, int fromSecond = 30)
        {
            return builder.AddPolicyHandler(ConfigureLinearHttpRetry(retryCount,fromSecond));
        }

        public static IHttpClientBuilder UseExpotentialHttpRetryPolicy(this IHttpClientBuilder builder, int retryCount = 3)
        {
            return builder.AddPolicyHandler(ConfigureExpotentialHttpRetry(retryCount));
        }

        public static IHttpClientBuilder UseCircuitBreakerPolicy(this IHttpClientBuilder builder, int eventsAllowBeforeBreaking = 3,
            int fromSeconds = 30)
        {
            return builder.AddPolicyHandler(ConfigureCircuitBreakerPolicy(eventsAllowBeforeBreaking, fromSeconds));
        }

        public static IHttpClientBuilder ConfigTimeOutPolicy(this IHttpClientBuilder builder, int seconds = 5)
        {
            return builder.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(seconds));
        }



       private static IAsyncPolicy<HttpResponseMessage> ConfigureImmediateHttpRetry(int retryCount) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .RetryAsync(retryCount, (exception, retryCount,context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception.Exception.Message}");
                });

        private static IAsyncPolicy<HttpResponseMessage> ConfigureLinearHttpRetry(int retryCount, int fromSecond) =>
          HttpPolicyExtensions
              .HandleTransientHttpError()
              .Or<TimeoutRejectedException>()
              .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(fromSecond), (exception, retryCount, context) =>
              {
                  Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception.Exception.Message}");
              });

        private static IAsyncPolicy<HttpResponseMessage> ConfigureExpotentialHttpRetry(int retryCount) =>
          HttpPolicyExtensions
              .HandleTransientHttpError()
              .Or<TimeoutRejectedException>()
              .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, retryCount, context) =>
              {
                  Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception.Exception.Message}");
              });


        private static IAsyncPolicy<HttpResponseMessage> ConfigureCircuitBreakerPolicy(int eventsAllowBeforeBreaking, int fromSeconds) =>
            HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: eventsAllowBeforeBreaking,
                durationOfBreak: TimeSpan.FromSeconds(fromSeconds));
    }
}
