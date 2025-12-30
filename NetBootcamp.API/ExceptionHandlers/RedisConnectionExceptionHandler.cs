using Microsoft.AspNetCore.Diagnostics;
using NetBootcamp.Services.SharedDTOs;
using System.Net;

namespace NetBootcamp.API.ExceptionHandlers
{
    public class RedisConnectionExceptionHandler(ILogger<RedisConnectionExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is StackExchange.Redis.RedisConnectionException)
            {
                logger.LogError(exception, "Redis connection exception occurred.");

                var responseModel = ResponseModelDto<string>.Fail("Service is temporarily unavailable due to Redis connection issues.", HttpStatusCode.ServiceUnavailable);
                await httpContext.Response.WriteAsJsonAsync(responseModel);
            }

            return true;
        }
    }
}
