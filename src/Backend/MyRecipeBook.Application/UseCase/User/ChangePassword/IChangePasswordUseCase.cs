using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
