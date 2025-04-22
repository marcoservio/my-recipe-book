namespace MyRecipeBook.Application.UseCases.Login.RequestCode;

public interface IRequestCodeResetPasswordUseCase
{
    Task Execute(string email);
}
