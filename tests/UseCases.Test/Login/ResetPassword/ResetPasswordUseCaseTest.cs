using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Login.ResetPassword;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login.ResetPassword;

public class ResetPasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var code = CodeToPerformActionBuilder.Build(user.Id);
        var request = RequestResetYourPasswordJsonBuilder.Build(user.Email);

        var useCase = CreateUseCase(user, code);

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Code_Not_Found()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestResetYourPasswordJsonBuilder.Build(user.Email);

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<OnValidationException>())
             .Where(e => e.GetErrorMessages().Count == 1 &&
                         e.GetErrorMessages().Contains(ResourceMessagesException.CODE_IS_NULL));
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var code = CodeToPerformActionBuilder.Build(1);
        var request = RequestResetYourPasswordJsonBuilder.Build();

        var useCase = CreateUseCase(code: code);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<OnValidationException>())
             .Where(e => e.GetErrorMessages().Count == 1 &&
                         e.GetErrorMessages().Contains(ResourceMessagesException.INVALID_CODE));
    }

    [Fact]
    public async Task Error_Email_Not_Match()
    {
        (var user, _) = UserBuilder.Build();
        var code = CodeToPerformActionBuilder.Build(user.Id);
        var request = RequestResetYourPasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, code);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<OnValidationException>())
             .Where(e => e.GetErrorMessages().Count == 1 &&
                         e.GetErrorMessages().Contains(ResourceMessagesException.INVALID_CODE));
    }

    [Fact]
    public async Task Error_Code_Expired()
    {
        (var user, _) = UserBuilder.Build();
        var code = CodeToPerformActionBuilder.Build(user.Id);
        code.CreatedOn = code.CreatedOn.AddHours(-MyRecipeBookRuleConstants.PASSWORD_RESET_CODE_VALIDITY_HOURS - 1);
        var request = RequestResetYourPasswordJsonBuilder.Build(user.Email);

        var useCase = CreateUseCase(user, code);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<OnValidationException>())
             .Where(e => e.GetErrorMessages().Count == 1 &&
                         e.GetErrorMessages().Contains(ResourceMessagesException.EXPIRED_CODE));
    }

    private static ResetPasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null, MyRecipeBook.Domain.Entities.CodeToPerformAction? code = null)
    {
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var codeRepository = new CodeToPerformActionRepositoryBuilder().GetByCode(code).Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new ResetPasswordUseCase(codeRepository, userUpdateOnlyRepository, passwordEncripter, unitOfWork);
    }
}
