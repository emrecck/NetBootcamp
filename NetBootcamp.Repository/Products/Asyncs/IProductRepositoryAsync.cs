namespace NetBootcamp.Repository.Products.Asyncs
{
    public interface IProductRepositoryAsync : IGenericRepository<Product>
    {
        Task UpdateProductNameAsync(string name, int id);
    }
}
