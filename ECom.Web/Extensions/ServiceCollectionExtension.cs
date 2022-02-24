using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using WebApp.RESTClients;

namespace ECom.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPollyPolicies(this IServiceCollection services)
        {
            var retryPolicy = GetRetryPolicy();
            var noOp = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
            services.AddHttpClient<IProductHttpClient, ProductHttpClient>()
                    .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp)
                    .AddPolicyHandler(GetCircuitBreakerPolicy());
            return services;
        }
        //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-circuit-breaker-pattern
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
