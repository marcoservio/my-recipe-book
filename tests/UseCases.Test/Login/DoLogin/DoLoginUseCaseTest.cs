﻿using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using MyRecipeBook.Application.UseCase.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

using Xunit;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();
        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(e => e.Message.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
    }

    private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var repositoryReadOnly = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (user is not null)
            repositoryReadOnly.GetByEmailAndPassword(user);

        return new DoLoginUseCase(repositoryReadOnly.Build(), passwordEncripter, accessTokenGenerator);
    }
}
