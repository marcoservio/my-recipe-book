using MyRecipeBook.Domain.Entities.Base;

namespace MyRecipeBook.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;    
    public Guid UserIdentifier { get; set; } = Guid.NewGuid();

    public virtual ICollection<Recipe> Recipes { get; set; } = [];
    public virtual ICollection<CodeToPerformAction> CodesToPerformAction { get; set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
