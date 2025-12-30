using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetBootcamp.API.ExceptionHandlers;
using NetBootcamp.Services;
using NetBootcamp.Services.Products.Configurations;
using NetBootcamp.Services.Redis;

namespace NetBootcamp.API.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // .net in kendi validasyon kontrol mekanizmasını devre dışı bırakıp kendi validasyon filterımızı controller a tanıtacağız.
        services.Configure<ApiBehaviorOptions>(x => {
            x.SuppressModelStateInvalidFilter = true;
        });

        services.AddAutoMapper(cfg => { }, typeof(ServiceAssembly).Assembly);

        // Fluent validation ı projeye ekler
        services.AddFluentValidationAutoValidation();

        // Çalıştığı assembly üzerindeki bütün abstract validator ları ekler
        //builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); 

        services.AddSingleton<RedisService>(r =>
        {
            return new RedisService(configuration.GetConnectionString("Redis")!);
        });
        services.AddProductService();

        services.AddExceptionHandler<RedisConnectionExceptionHandler>();
        services.AddExceptionHandler<CriticalExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}
