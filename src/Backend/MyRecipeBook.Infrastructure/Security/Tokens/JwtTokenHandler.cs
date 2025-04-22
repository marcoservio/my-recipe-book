using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens;

public abstract class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey) => new(Encoding.UTF8.GetBytes(signingKey));
}
