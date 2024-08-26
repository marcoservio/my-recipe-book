using System.Xml.Linq;

namespace MyRecipeBook.Application.UseCase.Login.External;

public interface IExternalLoginUseCase
{
    Task<string> Execute(string name, string email);
}
