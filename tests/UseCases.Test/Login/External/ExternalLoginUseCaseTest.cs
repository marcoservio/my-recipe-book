using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Login.External;

namespace UseCases.Test.Login.External;

public class ExternalLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(user.Name, user.Email);

        result.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Success_User_Not_Found()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(user.Name, user.Email);

        result.Should().NotBeNullOrWhiteSpace();
    }

    private static ExternalLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var writeOnlyRepositoryBuilder = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWorkBuilder = UnitOfWorkBuilder.Build();
        var jwtTokenGeneratorBuilder = JwtTokenGeneratorBuilder.Build();

        return new ExternalLoginUseCase(writeOnlyRepositoryBuilder, unitOfWorkBuilder, jwtTokenGeneratorBuilder, readOnlyRepositoryBuilder);
    }
}
