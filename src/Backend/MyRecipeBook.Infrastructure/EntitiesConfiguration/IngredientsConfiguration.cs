using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.EntitiesConfiguration;

public class IngredientsConfiguration : BaseEntityConfiguration<Ingredient>
{
    public override void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Item).IsRequired().HasMaxLength(100);

        builder.HasOne(r => r.Recipe)
            .WithMany(c => c.Ingredients)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
