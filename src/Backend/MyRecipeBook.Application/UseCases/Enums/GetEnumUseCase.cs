using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Enums.Attibutes;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Enums;

public class GetEnumUseCase : IGetEnumUseCase
{
    public ResponseEnumsJson Execute(string enumName)
    {
        var enumType = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic && a.FullName != null)
                .SelectMany(a => a.GetSafeTypes())
                .FirstOrDefault(t => t.IsEnum &&
                         string.Equals(t.Name, enumName, StringComparison.OrdinalIgnoreCase) &&
                         t.GetCustomAttributes(typeof(ExposeEnumAttribute), false).Length > 0);

        if (enumType == null || !enumType.IsEnum)
            throw new NotFoundException(ResourceMessagesException.ENUM_NOT_FOUND);

        var enumsList = Enum.GetValues(enumType)
                        .Cast<object>()
                        .Select(e => new ResponseEnumJson
                        {
                            Name = e.ToString()!,
                            Value = (int)e
                        })
                        .ToList();

        return new ResponseEnumsJson { Enums = enumsList };
    }
}
