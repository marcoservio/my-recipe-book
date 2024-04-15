﻿using Microsoft.EntityFrameworkCore;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;
public class UserRepository(MyRecipeBookDbContext context) : IUserWriteOnlyRepository, IUserReadOnlyRepository
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

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Equals(email) && user.Password.Equals(password));
    }
}
