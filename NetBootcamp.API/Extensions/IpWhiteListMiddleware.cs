using System.Net;

namespace NetBootcamp.API.Extensions
{
    public class IpWhiteListMiddleware(RequestDelegate next)
    {
        private List<IPAddress> _whitelist = [ IPAddress.Parse("::1"), IPAddress.Parse("127.0.0.1")];

        public async Task InvokeAsync(HttpContext context)
        {
            // check swagger ui path
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await next(context);
                return;
            }

            var remoteIp = context.Connection.RemoteIpAddress;
            if (!_whitelist.Contains(remoteIp))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Forbidden: Your IP is not allowed to access this resource.");
                return;
            }
            await next(context);
        }
    }
}
