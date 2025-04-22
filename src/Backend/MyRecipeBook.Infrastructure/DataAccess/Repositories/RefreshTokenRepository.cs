using Microsoft.EntityFrameworkCore;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository(MyRecipeBookDbContext context) : IRefreshTokenRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _context
            .RefreshTokens
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Value.Equals(refreshToken));
    }

    public async Task SaveNew(RefreshToken refreshToken)
    {
        var tokens = _context.RefreshTokens.Where(t => t.UserId == refreshToken.UserId);

        _context.RefreshTokens.RemoveRange(tokens);

        await _context.RefreshTokens.AddAsync(refreshToken);
    }
}
