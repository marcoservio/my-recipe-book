using Microsoft.EntityFrameworkCore;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;
public class UserRepository(MyRecipeBookDbContext context) : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email.Equals(email) && user.Active);
    }

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);
    }

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
    }

    public async Task<User> GetById(long id)
    {
        return await _context.Users.FirstAsync(user => user.Id.Equals(id));
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task DeleteAccount(Guid userIdentifier)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.UserIdentifier == userIdentifier);
        if (user is null)
            return;

        var recipes = _context.Recipes.Where(recipe => recipe.UserId == user.Id);

        _context.Recipes.RemoveRange(recipes);

        _context.Users.Remove(user);
    }
}
