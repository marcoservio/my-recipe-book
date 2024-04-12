namespace MyRecipeBook.Application.Services.Cryptography;
public interface IPasswordEncrypter
{
    string Encrypt(string password);
}
