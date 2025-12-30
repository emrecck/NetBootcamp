using NetBootcamp.Repository.Repositories;

namespace NetBootcamp.Repository.Products.Asyncs;

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
