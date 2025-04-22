namespace MyRecipeBook.Domain.Services.Email;

public interface ISendCodeResetPassword
{
    Task SendEmail(Entities.User user, string code);
}
