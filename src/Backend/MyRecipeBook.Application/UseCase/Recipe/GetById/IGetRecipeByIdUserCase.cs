using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.Recipe.GetById;

public interface IGetRecipeByIdUserCase
{
    Task<ResponseRecipeJson> Execute(long recipeId);
}
