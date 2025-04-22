using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> User();
}
