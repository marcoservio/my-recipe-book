using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build(int passwordLength = 10)
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(l => l.Email, (f) => f.Internet.Email())
            .RuleFor(l => l.Password, (f) => f.Internet.Password(passwordLength));
    }
}
