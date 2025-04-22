using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Domain.Extensions;

namespace MyRecipeBook.CrossCutting.Filters;

public class AuthenticatedUserFilter
    (IAccessTokenValidator accessTokenValidator, 
    IUserReadOnlyRepository userReadOnlyRepository) : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await _userReadOnlyRepository.ExistActiveUserWithIdentifier(userIdentifier);

            if (exist.IsFalse())
                throw new OnAuthorizationException();
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.TOKEN_EXPIRED)
            {
                TokenIsExpired = true
            });
        }
        catch (Exception ex) when (ex is OnAuthorizationException || ex is TokenOnRequestException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (authentication.Empty())
            throw new TokenOnRequestException();

        return authentication["Bearer ".Length..].Trim();
    }
}
