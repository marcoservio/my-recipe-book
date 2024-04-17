namespace MyRecipeBook.Communication.Responses;
public class ResponseRegisterUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokensJson Tokens { get; set; } = default!;
}
