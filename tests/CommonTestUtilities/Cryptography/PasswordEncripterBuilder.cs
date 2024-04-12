﻿using MyRecipeBook.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        return new PasswordEncripter("ABC1234");
    }
}
