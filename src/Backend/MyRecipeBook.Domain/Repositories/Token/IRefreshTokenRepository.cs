using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Token;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> Get(string refreshToken);
    Task SaveNew(RefreshToken refreshToken);
}
