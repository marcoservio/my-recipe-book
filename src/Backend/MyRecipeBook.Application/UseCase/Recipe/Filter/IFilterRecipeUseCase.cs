using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request);
}
