using Microsoft.AspNetCore.Http;

using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

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

        (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

        if (!isValidImage)
            throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

            _repository.Update(recipe);

            await _unitOfWord.Commit();
        }

        await _blobStorageService.Upload(logged, fileStream, recipe.ImageIdentifier);
    }
}
