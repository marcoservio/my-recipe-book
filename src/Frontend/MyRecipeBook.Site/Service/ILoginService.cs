using Microsoft.AspNetCore.Mvc;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

using Refit;

namespace MyRecipeBook.Site.Service;

public interface ILoginService
{
    [Post("/login")]
    Task<ResponseRegisteredUserJson> Login([FromBody] RequestLoginJson request);

    [Get("/login/google")]
    Task<string> LoginGoogle([Query] string redirectUrl);
}
