using Azure.Core;
using NetBootcamp.API.DTOs;
using NetBootcamp.API.Repositories;
using System.Linq.Expressions;
using System.Net;

namespace NetBootcamp.API.Products.Async
{
    public class ProductRepositoryAsync : GenericRepository<Product>, IProductRepositoryAsync
    {
        // Constructorlar kalıtım yoluyla aktarılmaz. Doalyısıyla AppDbContext i alan bir constructor yazıyoruz.
        public ProductRepositoryAsync(AppDbContext context) : base(context)
        {
        }

        public async Task UpdateProductNameAsync(string name, int id)
        {
            var product = await GetByIdAsync(id);
            product.Name = name;
            await UpdateAsync(product);
        }
    }
}
