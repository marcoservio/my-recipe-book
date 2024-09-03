using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;
public class UnitOfWork(MyRecipeBookDbContext context) : IUnitOfWork
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }   
}
