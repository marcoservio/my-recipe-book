namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class ErrorOnValidationException(IList<string> errorMessages) : MyRecipeBookException
{
    public IList<string> ErrorMessages { get; set; } = errorMessages;
}
