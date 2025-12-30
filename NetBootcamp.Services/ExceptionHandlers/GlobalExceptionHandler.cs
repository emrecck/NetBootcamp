using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NetBootcamp.Services.SharedDTOs;
using System.Net;

namespace NetBootcamp.Services.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var responseModel = ResponseModelDto<string>.Fail("An unexpected error occurred.", HttpStatusCode.InternalServerError);

        await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken);
        return true;
    }
}
