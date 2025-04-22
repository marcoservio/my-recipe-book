using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.Filter;

public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();

        var result = new FilterRecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((CookingTime)1000);

        var result = new FilterRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
    }

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.Difficulties.Add((Difficulty)1000);

        var result = new FilterRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = new FilterRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
    }
}
