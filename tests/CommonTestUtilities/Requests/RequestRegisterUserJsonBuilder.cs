using Bogus;

using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(x => x.Password, f => f.Internet.Password(passwordLength));
    }
}
