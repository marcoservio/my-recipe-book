using Bogus;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;

namespace CommonTestUtilities.Requests;

public class RequestResetYourPasswordJsonBuilder
{
    public static RequestResetYourPasswordJson Build(string email = "", int passwordLength = 10, string code = "")
    {
        if (email.Empty())
            email = new Faker().Internet.Email();

        if(code.Empty())
            code = Guid.NewGuid().ToString();

        return new Faker<RequestResetYourPasswordJson>()
            .RuleFor(u => u.Email, _ => email)
            .RuleFor(u => u.Code, _ => code)
            .RuleFor(u => u.NewPassword, (f) => f.Internet.Password(passwordLength));
    }
}
