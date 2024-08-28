using Microsoft.EntityFrameworkCore;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Token;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class TokenRepository(MyRecipeBookDbContext context) : ITokenRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _context
            .RefreshTokens
            .AsNoTracking()
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    }

    public async Task SaveNewRefreshToken(RefreshToken refreshToken)
    {
        var tokens = _context.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);
        
        _context.RefreshTokens.RemoveRange(tokens);

        await _context.RefreshTokens.AddAsync(refreshToken);
    }
}
