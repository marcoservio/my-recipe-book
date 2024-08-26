using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Application.UseCase.User.Delete.Request;

public class RequestDeleteUserUseCase(IDeleteUserQueue queue, IUserUpdateOnlyRepository userUpdateOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork) : IRequestDeleteUserUseCase
{
    private readonly IDeleteUserQueue _queue = queue;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository = userUpdateOnlyRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute()
    {
        var logged = await _loggedUser.User();

        var user = await _userUpdateOnlyRepository.GetById(logged.Id);

        user.Active = false;
        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();

        await _queue.SendMessage(logged);
    }
}
