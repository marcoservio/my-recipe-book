using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class OnAuthorizationException() : MyRecipeBookException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}