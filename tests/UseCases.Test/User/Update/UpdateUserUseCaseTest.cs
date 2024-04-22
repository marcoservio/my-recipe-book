using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCase.User.Update;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

using Xunit;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
            e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
            e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        if(email!.NotEmpty())
            readOnlyRepository.ExistActiveUserWithEmail(email);


        return new UpdateUserUseCase(loggedUser, updateOnlyRepository, readOnlyRepository.Build(), unitOfWork);
    }
}
