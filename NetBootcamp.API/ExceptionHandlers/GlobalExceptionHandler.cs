using Microsoft.AspNetCore.Diagnostics;
using NetBootcamp.Services.SharedDTOs;
using System.Net;

namespace NetBootcamp.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var responseModel = ResponseModelDto<string>.Fail("An unexpected error occurred.", HttpStatusCode.InternalServerError);

        await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);
        return true;
    }
}
