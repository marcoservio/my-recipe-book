using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

using Refit;

namespace MyRecipeBook.Site.Services;

public interface ILoginService
{
    [Post("/login")]
    Task<ResponseRegisteredUserJson> Login(RequestLoginJson request);

    [Get("/login/google")]
    Task<string> LoginGoogle(string redirectUrl);
}
