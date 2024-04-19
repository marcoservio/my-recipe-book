namespace MyRecipeBook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(Guid userIdentifierm);
}
