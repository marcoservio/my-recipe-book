using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Server.Requests;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public interface IRegisterRecipeUseCase
{
    Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request);
}
