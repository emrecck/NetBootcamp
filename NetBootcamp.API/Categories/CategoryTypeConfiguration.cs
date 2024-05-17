using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetBootcamp.API.Categories
{
    public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(category => category.Id);
            builder.Property(category => category.Id).ValueGeneratedNever();    // Db üretmesin, biz manuel ekleyeceğiz datayı.
            builder.Property(category => category.Name).IsRequired().HasMaxLength(100);
        }
    }
}
