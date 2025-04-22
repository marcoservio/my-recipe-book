using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.EntitiesConfiguration;

public class RecipeConfiguration : BaseEntityConfiguration<Recipe>
{
    public override void Configure(EntityTypeBuilder<Recipe> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Title).IsRequired().HasMaxLength(100);
        builder.Property(r => r.CookingTime);
        builder.Property(r => r.Difficulty);

        builder.HasOne(r => r.User)
            .WithMany(c => c.Recipes)
            .HasForeignKey(r => r.UserId);
    }
}
