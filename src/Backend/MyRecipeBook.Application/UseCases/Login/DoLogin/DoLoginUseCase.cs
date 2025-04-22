using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Refresh;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(IUserReadOnlyRepository readRepository,
    IPasswordEncripter passwordEncripter,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenCreator refreshTokenCreator) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _readRepository = readRepository;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenCreator _refreshTokenCreator = refreshTokenCreator;

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _readRepository.GetByEmail(request.Email);

        if (user is null || _passwordEncripter.IsValid(request.Password, user.Password).IsFalse())
            throw new InvalidLoginException();

        var accessToken = _accessTokenGenerator.Generate(user.UserIdentifier);
        var refreshToken = await _refreshTokenCreator.CreateAndStore(user);

        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }
        };
    }
}
