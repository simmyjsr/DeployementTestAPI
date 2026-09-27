using InventoryAPI.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryID);
            builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.CreatedAt).IsRequired();
        }
    }
}
