using NetBootcamp.API.Extensions;
using NetBootcamp.API.Filters;
using NetBootcamp.Repository.Repositories;
using NetBootcamp.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ValitationFilter ý controller a tanýtarak kendi validasyon kurallarýmýza göre bir Response döneceðiz  // her requestte controllerdan nesne oluþturur
builder.Services.AddControllers(x => x.Filters.Add<ValidationFilter>());
builder.Services.AddEndpointsApiExplorer();

// swagger oluþturmak için kullanýlýr
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.SeedDatabase();

app.AddMiddlewares();

app.Run();
