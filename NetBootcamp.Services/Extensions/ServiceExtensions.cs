using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBootcamp.Services.ExceptionHandlers;
using NetBootcamp.Services.Products.Configurations;
using NetBootcamp.Services.Redis;
using NetBootcamp.Services.Token;
using NetBootcamp.Services.Weather;
using TokenOptions = NetBootcamp.Services.Token.TokenOptions;

namespace NetBootcamp.Services.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // .net in kendi validasyon kontrol mekanizmasını devre dışı bırakıp kendi validasyon filterımızı controller a tanıtacağız.
        services.Configure<ApiBehaviorOptions>(x =>
        {
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

        services.AddScoped<IWeatherService, WeatherService>();
        services.AddScoped<ITokenService, TokenService>();

        // Options Pattern
        services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
        services.Configure<ClientCredentials>(configuration.GetSection("ClientCredentials"));
    }
}
