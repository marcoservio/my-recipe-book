using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class UnitOfWork(MyRecipeBookDbContext dbContext) : IUnitOfWork
{
    private readonly MyRecipeBookDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
