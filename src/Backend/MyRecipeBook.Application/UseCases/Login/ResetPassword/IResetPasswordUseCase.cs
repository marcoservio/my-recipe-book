using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.Login.ResetPassword;

public interface IResetPasswordUseCase
{
    Task Execute(RequestResetYourPasswordJson request);
}
