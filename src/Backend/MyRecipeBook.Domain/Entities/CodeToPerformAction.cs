using MyRecipeBook.Domain.Entities.Base;

namespace MyRecipeBook.Domain.Entities;

public class CodeToPerformAction : EntityBase
{
    public string Value { get; set; } = string.Empty;

    public long UserId { get; set; }
    public virtual User? User { get; set; }
}
