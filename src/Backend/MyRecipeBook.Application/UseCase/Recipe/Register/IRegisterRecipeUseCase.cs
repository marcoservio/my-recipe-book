using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.Recipe.Register;

public interface IRegisterRecipeUseCase
{
    Task<ResponseRegisterRecipeJson> Execute(RequestRegisterRecipeFormData request);
}
