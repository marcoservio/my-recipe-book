using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    Task Execute(long recipeId, RequestRecipeJson request);
}
