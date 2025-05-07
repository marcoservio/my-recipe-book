using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

using Refit;

namespace MyRecipeBook.Site.Services;

public interface IUserService
{
    [Post("/users")]
    Task<ResponseRegisteredUserJson> Register(RequestRegisterUserJson request);

    [Get("/users")]
    Task<ResponseUserProfileJson> GetUserProfile();
}
