using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetBootcamp.Repository.Products
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(p => p.Created).IsRequired();
            builder.Property(p => p.Barcode).IsRequired().HasMaxLength(100);

            builder.HasData(new Product()
            {
                Id = 1,
                Name = "PC",
                Price = 800,
                Created = DateTime.Now,
                Barcode = Guid.NewGuid().ToString()
            },
            new Product()
            {
                Id = 2,
                Name = "Monitor",
                Price = 450,
                Created = DateTime.Now,
                Barcode = Guid.NewGuid().ToString()
            });
        }
    }
}
