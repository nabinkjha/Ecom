using ECom.Contracts.Data.Repositories;
using ECom.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECom.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApiKeyValidatorsMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiKeyValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork uow)
        {
            var ignorePath = new List<string> { "/swagger/v1/swagger.json", "/swagger/v2/swagger.json", "/index.html" };
            if (!ignorePath.Contains(context.Request.Path.Value))// to ignore Swagger UI
            {
                if (!context.Request.Headers.Keys.Contains("api-key"))
                {
                    throw new MissingApiKeyException("API Key is missing"); ;
                }
                else if (!uow.ApiClient.CheckValidApiKey(context.Request.Headers["api-key"]))
                {
                    throw new InvalidApiKeyException("Invalid API Key");
                }
            }
            await _next.Invoke(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyValidatorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyValidatorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyValidatorsMiddleware>();
        }
    }
}
