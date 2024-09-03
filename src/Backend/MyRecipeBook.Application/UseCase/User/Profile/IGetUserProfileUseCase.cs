using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
