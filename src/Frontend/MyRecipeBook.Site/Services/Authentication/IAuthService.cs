using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Site.Services.Authentication;

public interface IAuthService
{
    Task Login(RequestLoginJson request);
    Task<string?> GetToken();
    Task Logout();
}
