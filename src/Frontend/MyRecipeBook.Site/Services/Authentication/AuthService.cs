using Blazored.LocalStorage;

using ClassLibrary1.Enums;

using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Site.Services.Authentication;

public class AuthService(ILoginService loginService, ILocalStorageService localStorage) : IAuthService
{
    private readonly ILoginService _loginService = loginService;
    private readonly ILocalStorageService _localStorage = localStorage;

    public async Task Login(RequestLoginJson request)
    {
        try
        {
            var response = await _loginService.Login(request);

            var token = response?.Tokens.AccessToken;

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Token is not found");

            await _localStorage.SetItemAsync(nameof(LocalStorageKey.auth_token), token);
        }
        catch
        {
            throw;
        }
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync<string>(nameof(LocalStorageKey.auth_token));
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(nameof(LocalStorageKey.auth_token));
    }
}
