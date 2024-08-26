using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCase.User.Delete.Delete;

public class DeleteUserAccountUseCase(IUserDeleteOnlyRepository repository, IUnitOfWork unitOfWork, IBlobStorageService bobStorageService) : IDeleteUserAccountUseCase
{
    private readonly IUserDeleteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBlobStorageService _blobStorageService = bobStorageService;

    public async Task Execute(Guid userIdentifier)
    {
        await _blobStorageService.DeleteContainer(userIdentifier);

        await _repository.DeleteAccount(userIdentifier);

        await _unitOfWork.Commit();
    }
}
