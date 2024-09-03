using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(IList<string> errorMessages) : MyRecipeBookException(string.Empty)
{
    private readonly IList<string> _errorMessages = errorMessages;

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
