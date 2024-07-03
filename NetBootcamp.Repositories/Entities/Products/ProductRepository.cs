namespace NetBootcamp.Repositories.Entities.Products
{
    public class ProductRepository(EFDbContext context) : GenericRepository<Product>(context) ,IProductRepository
    {
    }
}
