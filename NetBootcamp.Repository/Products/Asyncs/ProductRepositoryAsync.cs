using Azure.Core;
using NetBootcamp.Repository.Categories;
using System.Linq.Expressions;
using System.Net;

namespace NetBootcamp.Repository.Products.Asyncs
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

            #region Manuel Transaction Way
            /*  Bussiness da iki save changes kullanmak gerekiyorsa ve ilk save changes den sonra id, diğer işlemde kullanılıyorsa
             *  bu durumda bütünlüğü korumak için Manuel Transaction oluşturulur.
             */

            //using (var transaction = Context.Database.BeginTransaction())
            //{
            //    var category = new Category() { Name = "Kalemler" };
                
            //    Context.Categories.Add(category);

            //    Context.SaveChanges();

            //    var product = new Product() { CategoryId = category.Id };

            //    Context.Products.Add(product);

            //    Context.SaveChanges();

            //    transaction.Commit();
            }
            #endregion
        }
    }
}
