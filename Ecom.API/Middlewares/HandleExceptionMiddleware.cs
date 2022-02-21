using ECom.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ECom.API.Middlewares
{
    public class HandleExceptionMiddleware : IMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<HandleExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public HandleExceptionMiddleware(RequestDelegate next,
            ILogger<HandleExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
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
                if (env.IsDevelopment())
                {
                    exceptionResponse = new ApiException((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else
                {
                    exceptionResponse = new ApiException((int)statusCode, errorMessage);
                }

                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(exceptionResponse.ToString());
            }
        }
    }
}
