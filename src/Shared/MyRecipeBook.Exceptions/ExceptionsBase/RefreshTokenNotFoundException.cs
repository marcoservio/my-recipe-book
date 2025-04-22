using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class RefreshTokenNotFoundException() : MyRecipeBookException(ResourceMessagesException.INVALID_SESSION)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
