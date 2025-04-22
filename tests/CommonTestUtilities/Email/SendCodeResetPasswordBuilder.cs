using Moq;

using MyRecipeBook.Domain.Services.Email;

namespace CommonTestUtilities.Email;

public class SendCodeResetPasswordBuilder
{
    public static ISendCodeResetPassword Build()
    {
        var mock = new Mock<ISendCodeResetPassword>();

        return mock.Object;
    }
}
