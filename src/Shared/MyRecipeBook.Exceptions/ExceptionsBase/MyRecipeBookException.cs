using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public abstract class MyRecipeBookException(string message) : SystemException(message)
{
    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
