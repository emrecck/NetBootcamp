using Microsoft.AspNetCore.Authentication.Cookies;
using NetBootcamp.Web.Services.Token;
using NetBootcamp.Web.Services.User;
using NetBootcamp.Web.Services.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));

builder.Services.AddHttpClient<TokenService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Microservices:BaseUrl"]!);
});
builder.Services.AddHttpClient<WeatherService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Microservices:BaseUrl"]!);
}).AddHttpMessageHandler<ClientCredentialTokenInterceptor>();
builder.Services.AddHttpClient<UserService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Microservices:BaseUrl"]!);
}).AddHttpMessageHandler<ClientCredentialTokenInterceptor>();

builder.Services.AddScoped<ClientCredentialTokenInterceptor>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Auth/SignIn";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "AuthenticationCookie";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });
builder.Services.AddDataProtection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
