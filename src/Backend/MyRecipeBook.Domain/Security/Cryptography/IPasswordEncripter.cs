namespace MyRecipeBook.Domain.Security.Cryptography;

public interface IPasswordEncripter
{
    string Encrypt(string password);
    public bool IsValid(string password, string passwordHash);
}
