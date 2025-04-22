using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(x => x.Email, (f, user) => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password());
    }
}
