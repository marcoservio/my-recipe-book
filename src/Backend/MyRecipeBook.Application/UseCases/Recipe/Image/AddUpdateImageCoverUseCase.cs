using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

using Microsoft.AspNetCore.Http;

using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Image;

public class AddUpdateImageCoverUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository updateOnlyRepository, IUnitOfWork unitOfWork, IBlobStorageService blobStorageService) : IAddUpdateImageCoverUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task Execute(long recipeId, IFormFile file)
    {
        var authenticatedUser = await _loggedUser.User();

        var recipe = await _updateOnlyRepository.GetById(authenticatedUser, recipeId) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        var fileStream = file.OpenReadStream();

        (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

        if (isValidImage.IsFalse())
            throw new OnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

        if(recipe.ImageIdentifier.Empty())
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

            _updateOnlyRepository.Update(recipe);

            await _unitOfWork.Commit();
        }

        fileStream.Position = 0;

        await _blobStorageService.Upload(authenticatedUser, fileStream, recipe.ImageIdentifier!);
    }
}
