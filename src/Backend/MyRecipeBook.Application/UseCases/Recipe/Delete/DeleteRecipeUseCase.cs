
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public class DeleteRecipeUseCase(IRecipeReadOnlyRepository readOnlyRepository, IRecipeWriteOnlyRepository writeOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IDeleteRecipeUseCase
{
    protected readonly IRecipeReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    protected readonly IRecipeWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    protected readonly ILoggedUser _loggedUser = loggedUser;
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task Execute(long recipeId)
    {
        var authenticatedUser = await _loggedUser.User();

        var recipe = await _readOnlyRepository.GetById(authenticatedUser, recipeId) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        if (recipe.ImageIdentifier.NotEmpty())
            await _blobStorageService.Delete(authenticatedUser, recipe.ImageIdentifier!);

        await _writeOnlyRepository.Delete(recipe.Id);

        await _unitOfWork.Commit();
    }
}
