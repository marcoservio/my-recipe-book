using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRecipeBook.Exceptions;
using Microsoft.AspNetCore.Http;
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCase.Recipe.Image;

public class AddUpdateImageCoverUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository repositoryRead, IUnitOfWork unitOfWord, IBlobStorageService blobStorageService) : IAddUpdateImageCoverUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository = repositoryRead;
    private readonly IUnitOfWork _unitOfWord = unitOfWord;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task Execute(long recipeId, IFormFile file)
    {
        var logged = await _loggedUser.User();

        var recipe = await _repository.GetById(logged, recipeId) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        var fileStream = file.OpenReadStream();

        if (fileStream.Is<PortableNetworkGraphic>().IsFalse() && fileStream.Is<JointPhotographicExpertsGroup>().IsFalse())
            throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            _repository.Update(recipe);

            await _unitOfWord.Commit();
        }

        fileStream.Position = 0;

        await _blobStorageService.Upload(logged, fileStream, recipe.ImageIdentifier);
    }
}
