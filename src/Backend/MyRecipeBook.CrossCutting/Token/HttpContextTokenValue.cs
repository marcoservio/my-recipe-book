using Microsoft.AspNetCore.Http;

using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.CrossCutting.Token;

public class HttpContextTokenValue(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string Value()
    {
        var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authentication["Bearer ".Length..].Trim();
    }
}
