using Bogus;

using CommonTestUtilities.Tokens;

using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class RefreshTokenBuilder
{
    public static RefreshToken Build(User user)
    {
        return new Faker<RefreshToken>()
            .RuleFor(x => x.Id, () => 1)
            .RuleFor(x => x.Value, () => RefreshTokenGeneratorBuilder.Build().Generate())
            .RuleFor(x => x.UserId, _ => user.Id)
            .RuleFor(x => x.User, _ =>  user);
    }
}
