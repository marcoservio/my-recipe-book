using MyRecipeBook.Domain.Security.Refresh;

using System.Security.Cryptography;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        randomBytes[0] = (byte)(randomBytes[0] & 0x7F);

        return Convert.ToBase64String(randomBytes)
            .Replace("=", string.Empty)
            .Replace("+", string.Empty)
            .Replace("/", string.Empty);
    }
}
