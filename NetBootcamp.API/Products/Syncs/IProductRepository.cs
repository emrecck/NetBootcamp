namespace NetBootcamp.API.Products.Syncs
{
    public interface IProductRepository
    {
        IReadOnlyList<Product> GetAll();
        IReadOnlyList<Product> GetByPaging(int page, int pageSize);
        Product? GetById(int id);
        bool IsExist(string productName);
        void Create(Product product);
        void Update(Product product);
        void UpdateProductName(string name, int id);
        void Delete(int id);
    }
}
