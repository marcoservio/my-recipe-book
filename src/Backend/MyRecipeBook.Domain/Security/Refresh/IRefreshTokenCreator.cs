using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Security.Refresh;

public interface IRefreshTokenCreator
{
    Task<string> CreateAndStore(User user);
}
