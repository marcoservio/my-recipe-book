using Microsoft.EntityFrameworkCore;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(MyRecipeBookDbContext context) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
{
    private readonly MyRecipeBookDbContext _context = context;

    public async Task Add(User user) => await _context.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await _context.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) =>
        await _context.Users.AnyAsync(u => u.UserIdentifier.Equals(userIdentifier) && u.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password) =>
        (await _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password)))!;

    public async Task<User> GetById(long id) => await _context.Users.FirstAsync(u => u.Id == id);

    public void Update(User user) => _context.Users.Update(user);

    public async Task DeleteAccount(Guid userIdentifier)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.UserIdentifier == userIdentifier);

        if (user is null)
            return;

        var recipes = _context.Recipes.Where(r => r.UserId == user.Id);

        _context.Recipes.RemoveRange(recipes);

        _context.Users.Remove(user);
    }

    public Task<User?> GetByEmail(string email)
    {
        return _context
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));
    }
}
