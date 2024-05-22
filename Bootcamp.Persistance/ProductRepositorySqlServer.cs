using Bootcamp.Application.Products;
using Bootcamp.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Persistance
{
    public class ProductRepositorySqlServer(AppDbContext context) : IProductRepository
    {
        // geçici kullanım ,
        public async Task<Product> Create(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> GetById(int id)
        {
            return await context.Products.FindAsync(id);
        }
    }
}
