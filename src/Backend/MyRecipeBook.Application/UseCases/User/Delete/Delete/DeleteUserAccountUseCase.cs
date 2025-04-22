using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.User.Delete.Delete;

public class DeleteUserAccountUseCase(IUserDeleteOnlyRepository deleteOnlyRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IDeleteUserAccountUseCase
{
    private readonly IUserDeleteOnlyRepository _deleteOnlyRepository = deleteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task Execute(Guid userIdentifier)
    {
        await _blobStorageService.DeleteContainer(userIdentifier);

        await _deleteOnlyRepository.DeleteAccount(userIdentifier);

        await _unitOfWork.Commit();
    }
}
