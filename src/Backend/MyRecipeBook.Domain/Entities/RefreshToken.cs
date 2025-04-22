using MyRecipeBook.Domain.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("RefreshTokens")]
public class RefreshToken : EntityBase
{
    public string Value { get; set; } = string.Empty;

    public long UserId { get; set; }
    public virtual User? User { get; set; }
}
