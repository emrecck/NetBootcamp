using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBootcamp.API.Filters;
using NetBootcamp.Core.CrossCuttingConcerns.Caching;
using NetBootcamp.Core.CrossCuttingConcerns.Caching.InMemory;
using NetBootcamp.Core.CrossCuttingConcerns.Caching.Redis;
using NetBootcamp.Repositories;
using NetBootcamp.Repositories.Entities.Products;
using NetBootcamp.Repositories.Entities.Roles;
using NetBootcamp.Repositories.Entities.Users;
using NetBootcamp.Service;
using NetBootcamp.Service.Products;
using NetBootcamp.Service.Products.Helpers;
using NetBootcamp.Service.Roles;
using NetBootcamp.Service.Users;
using NetBootcamp.Service.Users.UserCreateUseCase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; }); // .net in kendi validation kontrol mekanizmasýný devre dýþý býraktýk.
builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());  // her requestte controllerdan nesne oluþturur. //Valitation Filter ý global filter olarak ekledik.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // swagger oluþturmak için kullanýlýr
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateRequestDtoValidator>();
builder.Services.AddAutoMapper(typeof(ServiceAssembly).Assembly);

builder.Services.AddDbContext<EFDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), x =>
    {
        x.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name);
    });
});

// Add services to the container.

// DI ( Dependency Inversion ) Container
// IoC ( Inversion of Control )
// - Dependency Inversion / Inversion of Control Principles
// - Dependency Injection Design Pattern

// - DI / IoC Contaier Frameworks
// - Autofac
// - Ninject

// LifeCycles
// 1.AddSingleton
// 2.AddScoped (*)
// 3.AddTransient

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<CacheService, RedisService>(x =>
{
    return new RedisService(builder.Configuration.GetConnectionString("Redis")!);
});
//builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
//builder.Services.AddMemoryCache();
builder.Services.AddSingleton<PriceCalculator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
