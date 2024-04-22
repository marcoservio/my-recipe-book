﻿using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(l => l.Name, (f) => f.Person.FirstName)
            .RuleFor(l => l.Email, (f, user) => f.Internet.Email(user.Name));
    }
}
