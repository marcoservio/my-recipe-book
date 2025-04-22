using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.EntitiesConfiguration;

public class CodeToPerformActionConfiguration : BaseEntityConfiguration<CodeToPerformAction>
{
    public override void Configure(EntityTypeBuilder<CodeToPerformAction> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Value).IsRequired();

        builder.HasOne(r => r.User)
            .WithMany(c => c.CodesToPerformAction)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
