namespace NetBootcamp.API.Products
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = [
                new Product { Id = 1, Name = "Laptop", Price=41000},
                new Product { Id = 2, Name = "Phone", Price=63000},
                new Product { Id = 3, Name = "TV", Price=28000},
            ];

        public IReadOnlyList<Product> GetAll() => _products; // daha sonra değiştirlmesini engellemek add, remove işlemlerini engellemek için IReadOnlyList olarak döndürüldü.

        public Product? GetById(int id) => _products.Find(x => x.Id == id); // possible null reference i gidermek için nullable operatorunu ekledik Product nesnesinin yanına

        public void Create(Product product)
        {
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(x => x.Id == product.Id);

            _products[index] = product;
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            _products.Remove(product!); // buradaki ünlem ! productın null olamayacağını compilera söyler.
        }
    }
}
