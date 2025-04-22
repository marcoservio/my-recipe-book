using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Security.Refresh;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Token.RefreshToken;

public class UserRefreshTokenUseCase(IRefreshTokenRepository tokenRepository, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenCreator refreshTokenCreator) : IUserRefreshTokenUseCase
{
    private readonly IRefreshTokenRepository _tokenRepository = tokenRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenCreator _refreshTokenCreator = refreshTokenCreator;

    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken) ?? throw new RefreshTokenNotFoundException();

        var refereshTokenValidUntil = refreshToken.CreatedOn.AddDays(MyRecipeBookRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if (DateTime.Compare(refereshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var accessToken = _accessTokenGenerator.Generate(refreshToken.User!.UserIdentifier);
        var newRefreshToken = await _refreshTokenCreator.CreateAndStore(refreshToken.User);

        return new ResponseTokensJson
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }
}
