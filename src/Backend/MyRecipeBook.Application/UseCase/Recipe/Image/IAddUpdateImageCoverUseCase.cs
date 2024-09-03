using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Application.UseCase.Recipe.Image;

public interface IAddUpdateImageCoverUseCase
{
    Task Execute(long recipeId, IFormFile file);
}
