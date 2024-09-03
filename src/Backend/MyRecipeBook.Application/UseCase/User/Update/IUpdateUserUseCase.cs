using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
