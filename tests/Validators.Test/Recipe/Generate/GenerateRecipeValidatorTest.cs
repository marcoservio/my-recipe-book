using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.Generate;

public class GenerateRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build();

        var result = new GenerateRecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_More_Maximum_Ingredient()
    {
        var request = RequestGenerateRecipeJsonBuilder
            .Build(count: MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);

        var result = new GenerateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS);
    }

    [Fact]
    public void Error_Duplicated_Ingredient()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build(count: MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add(request.Ingredients.First());

        var result = new GenerateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Ingredient_Empty(string? ingredient)
    {
        var request = RequestGenerateRecipeJsonBuilder.Build(count: 1);
        request.Ingredients.Add(ingredient!);

        var result = new GenerateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.INGREDIENT_EMPTY);
    }

    [Fact]
    public void Error_Ingredient_Not_Following_Pattern()
    {
        var request = RequestGenerateRecipeJsonBuilder.Build(count: MyRecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE - 1);
        request.Ingredients.Add("This is an invalid ingredient because is too long");

        var result = new GenerateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
    }
}
