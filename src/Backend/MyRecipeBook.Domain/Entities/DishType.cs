using MyRecipeBook.Domain.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("DishTypes")]
public class DishType : EntityBase
{
    public Enums.DishType Type { get; set; }

    public virtual Recipe? Recipe { get; set; }
    public long RecipeId { get; set; }
}
