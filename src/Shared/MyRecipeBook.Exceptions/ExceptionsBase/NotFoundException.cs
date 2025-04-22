using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class NotFoundException(string message) : MyRecipeBookException(message)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
