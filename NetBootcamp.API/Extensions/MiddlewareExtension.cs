namespace NetBootcamp.API.Extensions;

public static class MiddlewareExtension
{
    public static void AddMiddlewares(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.UseMiddleware<IpWhiteListMiddleware>().UseWhen(context => !context.Request.Path.StartsWithSegments("/swagger"), config => {});

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}