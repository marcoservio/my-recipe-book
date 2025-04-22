using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Enums;

public interface IGetEnumUseCase
{
    ResponseEnumsJson Execute(string enumName);
}
