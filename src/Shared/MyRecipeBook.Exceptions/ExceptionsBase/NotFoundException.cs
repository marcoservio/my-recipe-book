namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class NotFoundException(string message) : MyRecipeBookException(message)
{
}
