using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.ServiceBus;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.User.Delete.Request;

namespace UseCases.Test.User.Delete.Request;

public class RequestDeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = useCase.Execute;

        await act.Should().NotThrowAsync();
    }

    private static RequestDeleteUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var deleteUserQueue = DeleteUserQueueBuilder.Build();
        var updateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new RequestDeleteUserUseCase(deleteUserQueue, updateRepository, loggedUser, unitOfWork);
    }
}
