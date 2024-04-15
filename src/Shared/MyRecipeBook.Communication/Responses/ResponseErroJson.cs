namespace MyRecipeBook.Communication.Responses;

public class ResponseErroJson
{
    public IList<string> Errors { get; set; }

    public ResponseErroJson(IList<string> errors) => Errors = errors;

    public ResponseErroJson(string error)
    {
        Errors = [error];
    }
}
