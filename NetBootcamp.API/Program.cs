using Microsoft.AspNetCore.Authorization;
using NetBootcamp.API.Extensions;
using NetBootcamp.API.Filters;
using NetBootcamp.Repository.Repositories;
using NetBootcamp.Services.Extensions;
using NetBootcamp.Services.Token.Policies.OverAge;

var builder = WebApplication.CreateBuilder(args);

// ValitationFilter ý controller a tanýtarak kendi validasyon kurallarýmýza göre bir Response döneceðiz  // her requestte controllerdan nesne oluþturur
builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());
builder.Services.AddEndpointsApiExplorer();

// swagger oluþturmak için kullanýlýr
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddScoped<IAuthorizationHandler, OverAgeRequirementHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("UpdatePolicy", policy => {
        policy.RequireClaim("update", "true");
    })
    .AddPolicy("Over18AgePolicy", policy =>
    {
        policy.Requirements.Add(new OverAgeRequirement() { Age = 18});
    });

var app = builder.Build();

app.SeedDatabase();

await app.SeedUsersAsync();

app.AddMiddlewares();

app.Run();
