using Moq;

using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();    

    public void ExistActiveUserWithEmail(string email) => 
        _repository.Setup(x => x.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

    public UserReadOnlyRepositoryBuilder GetByEmail(User? user)
    {
        if(user is not null)
            _repository.Setup(x => x.GetByEmail(user!.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
