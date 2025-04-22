using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class TokenOnRequestException() : MyRecipeBookException(ResourceMessagesException.NO_TOKEN)
{
    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
