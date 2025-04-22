using Bogus;

using CommonTestUtilities.Cryptography;

using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var password = new Faker().Internet.Password();

        var user =  new Faker<User>()
            .RuleFor(x => x.Id, () => 1)
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(x => x.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(x => x.Password, f => passwordEncripter.Encrypt(password));

        return (user, password);
    }
}
