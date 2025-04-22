using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.EntitiesConfiguration;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name).HasMaxLength(255).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
        builder.Property(c => c.Password).HasMaxLength(2000).IsRequired();
    }
}
