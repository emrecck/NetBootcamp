using NetBootcamp.API.Categories;

namespace NetBootcamp.API.Repositories
{
    public class SeedData
    {
        // Seed dataların bir kere çalıştırılmasını ve ilk DB oluşturulduğunda eklenmesini daha sonra migration yapıldığında çalıştırılmamasını istiyoruz.
        // O yüzden program.cs içerisinde çalıştırıyoruz.
        public static void SeedDatabase(AppDbContext context)
        {
            context.Database.EnsureCreated();   // DB nin oluştuğundan emin olmak için kullanılır. Eğer Db oluşturulmamışsa DB yi oluşturur sonra Seed dataları ekler.

            if (context.Categories.Any())   // Categories tablosunda data var ise return edecek.
            {
                return;
            }

            var categories = new[]
            {
                new Category{ Id = Guid.NewGuid(), Name = "Electronics"},
                new Category{ Id = Guid.NewGuid(), Name = "Clothing"},
                new Category{ Id = Guid.NewGuid(), Name = "Grocery"},
                new Category{ Id = Guid.NewGuid(), Name = "Books"},
                new Category{ Id = Guid.NewGuid(), Name = "Furniture"},
            };

            context.Categories.AddRange(categories);    // Collection şeklinde datayı toplu şekilde context e eklemek için kullanılır.

            context.SaveChanges();
        }
    }
}
