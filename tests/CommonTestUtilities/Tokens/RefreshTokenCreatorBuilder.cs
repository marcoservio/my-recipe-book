using Moq;

using MyRecipeBook.Domain.Security.Refresh;

namespace CommonTestUtilities.Tokens;

public class RefreshTokenCreatorBuilder
{
    private readonly Mock<IRefreshTokenCreator> _repository;

    public RefreshTokenCreatorBuilder() => _repository = new Mock<IRefreshTokenCreator>();

    public RefreshTokenCreatorBuilder CreateAndStore()
    {
        var refreshToken = RefreshTokenGeneratorBuilder.Generate();

        _repository.Setup(x => x.CreateAndStore(It.IsAny<MyRecipeBook.Domain.Entities.User>()))
            .ReturnsAsync(refreshToken);

        return this;
    }

    public IRefreshTokenCreator Build() => _repository.Object;
}
