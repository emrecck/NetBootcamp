using Microsoft.Extensions.DependencyInjection;
using NetBootcamp.Repository.Products.Asyncs;
using NetBootcamp.Repository.Products.Syncs;
using NetBootcamp.Services.Products.Asyncs;
using NetBootcamp.Services.Products.Syncs;
using NetBootcamp.Services.Products.Helpers;
using NetBootcamp.Services.Products.ProductCreateUseCase;
using FluentValidation;
using NetBootcamp.API.Filters;

namespace NetBootcamp.Services.Products.Configurations
{
    public static class ProductServiceExtensions
    {
        public static void AddProductService(this IServiceCollection services)
        {
            services.AddScoped<NotFoundFilter>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductServiceAsync, ProductServiceAsync>();
            services.AddScoped<IProductRepositoryAsync, ProductRepositoryAsync>();

            services.AddSingleton<PriceCalculator>();

            services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();  // ProductCreateRequestValidator ın bulunduğu assembly i alır 
        }
    }
}
