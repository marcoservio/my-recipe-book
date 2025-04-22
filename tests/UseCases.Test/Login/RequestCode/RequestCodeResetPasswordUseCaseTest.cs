using CommonTestUtilities.Email;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Login.RequestCode;

namespace UseCases.Test.Login.RequestCode;

public class RequestCodeResetPasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(user.Email); };

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Success_User_Not_Found()
    {
        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute("email@notfound.com"); };

        await act.Should().NotThrowAsync();
    }

    private static RequestCodeResetPasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var codeRepository = new CodeToPerformActionRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var emailSender = SendCodeResetPasswordBuilder.Build();

        return new RequestCodeResetPasswordUseCase(userReadOnlyRepository, codeRepository, unitOfWork, emailSender);
    }
}
