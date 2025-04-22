using Moq;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories;

public class RefreshTokenRepositoryBuilder
{
    private readonly Mock<IRefreshTokenRepository> _repository;

    public RefreshTokenRepositoryBuilder() => _repository = new Mock<IRefreshTokenRepository>();

    public RefreshTokenRepositoryBuilder Get(RefreshToken? refreshToken)
    {
        if (refreshToken is not null)
            _repository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(refreshToken);

        return this;
    }

    public IRefreshTokenRepository Build() => _repository.Object;
}
