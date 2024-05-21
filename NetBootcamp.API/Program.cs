using NetBootcamp.Services.Products.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBootcamp.API.Filters;
using NetBootcamp.Repository;
using System.Reflection;
using NetBootcamp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), x =>
    {
        x.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name);
    });
});

builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; }); // .net in kendi validasyon kontrol mekanizmasýný devre dýþý býrakýp kendi validasyon filterýmýzý controller a tanýtacaðýz.

builder.Services.AddAutoMapper(typeof(ServiceAssembly).Assembly);
builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>()); // ValitationFilter ý controller a tanýtarak kendi validasyon kurallarýmýza göre bir Response döneceðiz  // her requestte controllerdan nesne oluþturur
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // swagger oluþturmak için kullanýlýr
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());    // Çalýþtýðý assembly üzerindeki bütün abstract validator larý ekler

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
builder.Services.AddProductService();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // generic birden fazla entity alýrsa burada "," ekliyoruz.

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
