using Microsoft.EntityFrameworkCore;
using NetBootcamp.Repository.Categories;
using NetBootcamp.Repository.Products;
using System.Reflection;

namespace NetBootcamp.Repository
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Current assembly içerisindeki tüm type configuration file ları ekler
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //modelBuilder.Entity<Product>(x =>
            //{
            //    x.HasKey(p => p.Id);
            //    x.Property(p => p.Name).IsRequired().HasMaxLength(100);
            //    x.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
            //    x.Property(p => p.Created).IsRequired();
            //    x.Property(p => p.Barcode).IsRequired().HasMaxLength(100);
            //});

            // modelBuilder.Entity<Product>().ToTable("Products");  Tablo isimlendirme

            base.OnModelCreating(modelBuilder);
        }
    }
}
