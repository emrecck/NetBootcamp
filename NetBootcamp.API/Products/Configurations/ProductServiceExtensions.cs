using FluentValidation;
using NetBootcamp.API.Filters;
using NetBootcamp.API.Products.Async;
using NetBootcamp.API.Products.Helpers;
using NetBootcamp.API.Products.ProductCreateUseCase;
using NetBootcamp.API.Products.Syncs;
using NetBootcamp.API.Repositories;

namespace NetBootcamp.API.Products.Configurations
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
