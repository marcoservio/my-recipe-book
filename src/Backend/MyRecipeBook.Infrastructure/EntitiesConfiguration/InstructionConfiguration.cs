using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.EntitiesConfiguration;
public class InstructionConfiguration : BaseEntityConfiguration<Instruction>
{
    public override void Configure(EntityTypeBuilder<Instruction> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Step).IsRequired();
        builder.Property(r => r.Text).IsRequired().HasMaxLength(2000);

        builder.HasOne(r => r.Recipe)
            .WithMany(c => c.Instructions)
            .HasForeignKey(r => r.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
