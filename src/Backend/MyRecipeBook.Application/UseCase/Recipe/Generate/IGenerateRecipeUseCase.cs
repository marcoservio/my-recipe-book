using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.Recipe.Generate;

public interface IGenerateRecipeUseCase
{
    Task<ResponseGeneratedRecipeJson> Execute(RequestGenerateRecipeJson request);
}
