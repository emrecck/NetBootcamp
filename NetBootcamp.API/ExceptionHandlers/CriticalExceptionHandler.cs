using Microsoft.AspNetCore.Diagnostics;

namespace NetBootcamp.API.ExceptionHandlers
{
    public class CriticalExceptionHandler(ILogger<CriticalExceptionHandler> logger) : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Burada CriticalException ı yakalayıp özel işlemler yapabiliriz.
            if (exception is CriticalException)
            {
                logger.LogError(exception, "Critical exception occurred.");
            }

            return new ValueTask<bool>(false);
        }
    }
}
