using Bogus;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public class CodeToPerformActionBuilder
{
    public static CodeToPerformAction Build(long userId)
    {
        var code = new Faker<CodeToPerformAction>()
            .RuleFor(x => x.Id, () => 1)
            .RuleFor(x => x.Value, _ => Guid.NewGuid().ToString())
            .RuleFor(x => x.UserId, _ => userId)
            .RuleFor(x => x.CreatedOn, _ => DateTime.UtcNow.AddHours(MyRecipeBookRuleConstants.PASSWORD_RESET_CODE_VALIDITY_HOURS));

        return code;
    }
}
