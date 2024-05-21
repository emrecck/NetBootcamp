using NetBootcamp.API.Repositories;

namespace NetBootcamp.API.Products.Async
{
    public interface IProductRepositoryAsync :  IGenericRepository<Product>
    {
        Task UpdateProductNameAsync(string name, int id);
    }
}
