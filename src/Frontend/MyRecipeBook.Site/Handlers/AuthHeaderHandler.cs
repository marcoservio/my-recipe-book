using Blazored.LocalStorage;

using ClassLibrary1.Enums;

using System.Net.Http.Headers;

namespace MyRecipeBook.Site.Handlers;

public class AuthHeaderHandler(ILocalStorageService localStorage) : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage = localStorage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>(nameof(LocalStorageKey.auth_token), cancellationToken);

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}

