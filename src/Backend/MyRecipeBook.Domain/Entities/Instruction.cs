using MyRecipeBook.Domain.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("Instructions")]
public class Instruction : EntityBase
{
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;

    public virtual Recipe? Recipe { get; set; }
    public long RecipeId { get; set; }
}
