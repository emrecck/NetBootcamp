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

builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; }); // .net in kendi validasyon kontrol mekanizmas�n� devre d��� b�rak�p kendi validasyon filter�m�z� controller a tan�taca��z.

builder.Services.AddAutoMapper(typeof(ServiceAssembly).Assembly);
builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>()); // ValitationFilter � controller a tan�tarak kendi validasyon kurallar�m�za g�re bir Response d�nece�iz  // her requestte controllerdan nesne olu�turur
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // swagger olu�turmak i�in kullan�l�r
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());    // �al��t��� assembly �zerindeki b�t�n abstract validator lar� ekler

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
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // generic birden fazla entity al�rsa burada "," ekliyoruz.

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
