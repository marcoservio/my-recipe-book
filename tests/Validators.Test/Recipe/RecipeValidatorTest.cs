using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCase.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

using Xunit;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_Cooking_Time_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_Difficulty_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_DishTypes_Empty()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = (CookingTime)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = (Difficulty)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Empty_Ingredients()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
    }

    [Fact]
    public void Error_Empty_Instructions()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION));
    }

    [Fact]
    public void Error_Same_Step_Instructions()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
    }

    [Fact]
    public void Error_Negative_Step_Instructions()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = -1;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.GREATER_THAN_ZERO_INSTRUCTION_STEP));
    }

    [Fact]
    public void Error_Instruction_Too_Long()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData("")]
    public void Error_Empty_Title(string? title)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.RECIPE_TITLE_EMPTY));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData("")]
    public void Error_Empty_Value_Ingredients(string? ingredient)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Add(ingredient!);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INGREDIENT_EMPTY));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData("")]
    public void Error_Empty_Value_Instruction(string? instruction)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = instruction!;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EMPTY));
    }
}
