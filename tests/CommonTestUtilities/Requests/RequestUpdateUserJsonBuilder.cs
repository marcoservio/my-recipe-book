using Bogus;

using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.Name));
    }
}
