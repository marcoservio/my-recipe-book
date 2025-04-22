using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.EntitiesConfiguration;

public class DishTypeConfiguration : BaseEntityConfiguration<DishType>
{
    public override void Configure(EntityTypeBuilder<DishType> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Type).IsRequired();

        builder.HasOne(r => r.Recipe)
            .WithMany(c => c.DishTypes)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
