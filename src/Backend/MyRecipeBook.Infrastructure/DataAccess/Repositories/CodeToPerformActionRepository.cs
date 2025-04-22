using Microsoft.EntityFrameworkCore;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.CodeToPerformAction;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class CodeToPerformActionRepository(MyRecipeBookDbContext context) : ICodeToPerformActionRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task Add(CodeToPerformAction code) => await _context.CodesToPerformAction.AddAsync(code);

    public async Task DeleteAllUserCodes(long userId) 
    {
        var codigos = await _context.CodesToPerformAction
            .Where(c => c.UserId == userId)
            .ToListAsync();

        _context.CodesToPerformAction.RemoveRange(codigos);
    }

    public async Task<CodeToPerformAction?> GetByCode(string code) => await
        _context
        .CodesToPerformAction
        .FirstOrDefaultAsync(x => x.Active && x.Value.Equals(code));
}
