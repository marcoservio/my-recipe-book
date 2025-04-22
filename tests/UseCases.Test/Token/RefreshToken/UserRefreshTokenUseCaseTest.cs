using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Token.RefreshToken;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Token.RefreshToken;

public class UserRefreshTokenUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);

        var useCase = CreateUseCase(refreshToken);

        var result = await useCase.Execute(new RequestNewTokenJson { RefreshToken = refreshToken.Value });

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_NotFound_RefreshToken()
    {
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(new RequestNewTokenJson { RefreshToken = "refreshToken" });

        await act.Should().ThrowAsync<RefreshTokenNotFoundException>()
            .Where(e => e.Message.Equals(ResourceMessagesException.INVALID_SESSION));
    }

    [Fact]
    public async Task Error_Expired_RefreshToken()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        refreshToken.CreatedOn = refreshToken.CreatedOn.AddDays(-MyRecipeBookRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS - 1);

        var useCase = CreateUseCase(refreshToken);

        Func<Task> act = async () => await useCase.Execute(new RequestNewTokenJson { RefreshToken = refreshToken.Value });

        await act.Should().ThrowAsync<RefreshTokenExpiredException>()
            .Where(e => e.Message.Equals(ResourceMessagesException.EXPIRED_SESSION));
    }

    private static UserRefreshTokenUseCase CreateUseCase(MyRecipeBook.Domain.Entities.RefreshToken? refreshToken = null)
    {
        var refreshTokenRepository = new RefreshTokenRepositoryBuilder().Get(refreshToken).Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenCreator = new RefreshTokenCreatorBuilder().CreateAndStore().Build();

        return new UserRefreshTokenUseCase(refreshTokenRepository, accessTokenGenerator, refreshTokenCreator);
    }
}
