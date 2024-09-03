using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class InvalidLoginException() : MyRecipeBookException(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
