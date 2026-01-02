using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetBootcamp.Repository.Identity;
using NetBootcamp.Repository.Repositories;
using NetBootcamp.Services.ExceptionHandlers;
using NetBootcamp.Services.Products.Configurations;
using NetBootcamp.Services.Redis;
using NetBootcamp.Services.Token;
using NetBootcamp.Services.Users;
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
        services.AddScoped<IUserService, UserService>();

        // Options Pattern
        services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
        services.Configure<ClientCredentials>(configuration.GetSection("ClientCredentials"));

        services.AddIdentityServices();
        services.AddAuthenticationServices(configuration);
    }

    public static void AddIdentityServices(this IServiceCollection services)
    {
        // add to Identity
        // UserManager, SignInManager, RoleManager gibi servisleri ekler
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
        }).AddEntityFrameworkStores<AppDbContext>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // jwt Authentication 
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = tokenOptions.Issuer,
                ValidAudiences = tokenOptions.Audiences,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenOptions.SignatureKey)),

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
            };
            options.Authority = tokenOptions.Issuer;
        });
    }

    public static async Task SeedUsersAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        await UserSeedData.SeedUsersAsync(
            services.GetRequiredService<UserManager<AppUser>>(),
            services.GetRequiredService<RoleManager<AppRole>>()
        );
    }
}
