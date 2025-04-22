using MyRecipeBook.Domain.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("Ingredients")]
public class Ingredient : EntityBase
{
    public string Item { get; set; } = string.Empty;

    public virtual Recipe? Recipe { get; set; }
    public long RecipeId { get; set; }
}
