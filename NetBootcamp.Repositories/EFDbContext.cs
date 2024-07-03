using Microsoft.EntityFrameworkCore;
using NetBootcamp.Repositories.Entities.Products;
using NetBootcamp.Repositories.Entities.Roles;
using NetBootcamp.Repositories.Entities.Users;

namespace NetBootcamp.Repositories
{
    public class EFDbContext(DbContextOptions<EFDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(product =>
            {
                product.HasKey(p => p.Id); // Primary Key
                product.Property(p => p.Name).IsRequired().HasMaxLength(100);
                product.Property(p => p.Price).IsRequired().HasPrecision(18,2);
                product.Property(p => p.CreatedDate).IsRequired();
                product.Property(p => p.Barcode).IsRequired().HasMaxLength(100);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
