using MyRecipeBook.Domain.Security.Refresh;
using MyRecipeBook.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;

public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();

    public static string Generate()
    {
        var generator = Build();
        return generator.Generate();
    }
}
