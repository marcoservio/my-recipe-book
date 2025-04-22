using FluentAssertions;

using MyRecipeBook.Application.UseCases.Enums;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

using UseCases.Test.Enums.InlineDatas;

namespace UseCases.Test.Enums;
public class GetEnumUseCaseTest
{
    [Theory]
    [ClassData(typeof(EnumInlineData))]
    public void Success(string e)
    {
        var useCase = new GetEnumUseCase();

        var result = useCase.Execute(e);

        result.Should().NotBeNull();
        result.Enums.Should().NotBeNullOrEmpty();
        result.Enums.Should().HaveCountGreaterThan(0);
    }

    [Theory]
    [ClassData(typeof(EnumInlineData))]
    public void Success_Enum_Lowercase(string e)
    {
        var useCase = new GetEnumUseCase();

        var result = useCase.Execute(e.ToLower());

        result.Should().NotBeNull();
        result.Enums.Should().NotBeNullOrEmpty();
        result.Enums.Should().HaveCountGreaterThan(0);
    }

    [Theory]
    [ClassData(typeof(EnumNotFoundInlineData))]
    public void Error_Enum_NotFound(string e)
    {
        var useCase = new GetEnumUseCase();

        Action act = () => useCase.Execute(e);

        act.Should().Throw<NotFoundException>()
            .Where(ex => ex.Message.Equals(ResourceMessagesException.ENUM_NOT_FOUND));
    }
}
