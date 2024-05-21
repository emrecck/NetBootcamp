
using NetBootcamp.API.Products.Syncs;
using NetBootcamp.API.Repositories;

namespace NetBootcamp.API.Products
{
    public class ProductRepositoryWithSqlServerAsync(AppDbContext context) : IProductRepository
    {
        public void Create(Product product)
        {
            context.Products.Add(product);
        }

        public void Delete(int id)
        {
            var production = context.Products.Find(id);
            context.Products.Remove(production!);
        }

        public IReadOnlyList<Product> GetAll()
        {
            return context.Products.ToList().AsReadOnly();
        }

        public Product? GetById(int id)
        {
            return context.Products.Find(id);
        }

        public IReadOnlyList<Product> GetByPaging(int page, int pageSize)
        {
            return context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList().AsReadOnly();
        }

        public bool IsExist(string productName)
        {
            return context.Products.Any(product => product.Name == productName);
        }

        public void Update(Product product)
        {
            context.Products.Update(product);
        }

        public void UpdateProductName(string name, int id)
        {
            var product = GetById(id);
            product!.Name = name;
            context.Products.Update(product);
        }
    }
}
