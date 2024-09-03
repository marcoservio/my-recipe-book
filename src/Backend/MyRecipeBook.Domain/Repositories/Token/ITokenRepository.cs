using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Token;

public interface ITokenRepository
{
    Task<RefreshToken?> Get(string refreshToken);
    Task SaveNewRefreshToken(RefreshToken refreshToken);
}
