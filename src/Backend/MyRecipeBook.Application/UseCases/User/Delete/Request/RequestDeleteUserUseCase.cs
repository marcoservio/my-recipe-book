using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase(IDeleteUserQueue queue, IUserUpdateOnlyRepository updateOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork) : IRequestDeleteUserUseCase
{
    private readonly IDeleteUserQueue _queue = queue;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute()
    {
        var authenticatedUser =  await _loggedUser.User();

        var user = await _updateOnlyRepository.GetById(authenticatedUser.Id);

        user.Active = false;
        _updateOnlyRepository.Update(user);

        await _unitOfWork.Commit();

        await _queue.SendMessage(authenticatedUser);
    }
}
