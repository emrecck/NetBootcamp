using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NetBootcamp.API.Products;
using NetBootcamp.API.Products.DTOs.ProductCreateUseCase;
using NetBootcamp.API.Redis;
using NetBootcamp.API.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddSingleton<RedisService>(x =>
{
    return new RedisService(builder.Configuration.GetConnectionString("Redis")!);
});

builder.Services.AddControllers();  // her requestte controllerdan nesne oluþturur
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // swagger oluþturmak için kullanýlýr
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());    // Çalýþtýðý assembly üzerindeki bütün abstract validator larý ekler
builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();  // ProductCreateRequestValidator ýn bulunduðu assembly i alýr 

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
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<PriceCalculator>();

var app = builder.Build();

app.SeedDatabase(); // Extension method for WebApplication

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
