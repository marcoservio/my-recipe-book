using MyRecipeBook.Domain.Entities.Base;
using MyRecipeBook.Enums;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public string? ImageIdentifier { get; set; }

    public virtual User? User { get; set; }
    public long UserId { get; set; }

    public virtual IList<Ingredient> Ingredients { get; set; } = [];
    public virtual IList<Instruction> Instructions { get; set; } = [];
    public virtual IList<DishType> DishTypes { get; set; } = [];
}
