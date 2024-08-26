using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public void GetByEmailAndPassword(MyRecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
    }

    public void GetByEmail(MyRecipeBook.Domain.Entities.User user)
    {
        _repository.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
