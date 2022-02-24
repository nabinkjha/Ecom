using ECom.API.Utilities;
using ECom.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ECom.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionHandlerMiddleware(RequestDelegate next,ILogger<ExceptionHandlerMiddleware> logger,
            IHostEnvironment env)
        {
            this._next = next;
            this.logger = logger;
            this.env = env;
        }
        

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                ApiException exceptionResponse;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                string errorMessage = "Some error occured in the server.";
                var exceptionType = ex.GetType();
                if (exceptionType == typeof(UnauthorizedAccessException))
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    errorMessage = "Unauthorize access";
                }
                else if (exceptionType == typeof(MissingApiKeyException))
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    errorMessage = ex.Message;
                }
                else if (exceptionType == typeof(InvalidApiKeyException))
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    errorMessage = ex.Message;
                }
                if (env.IsDevelopment())
                {
                    exceptionResponse = new ApiException((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else
                {
                    exceptionResponse = new ApiException((int)statusCode, errorMessage);
                }
                logger.LogError(ex, ex.Message);
                httpContext.Response.StatusCode = (int)statusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(exceptionResponse.ToString());
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
