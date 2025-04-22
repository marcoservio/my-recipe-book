using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_Cooking_Time_Null()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = null;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_Difficulty_Null()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = null;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_DishTypes_Empty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Title_Empty(string? title)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title!;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.RECIPE_TITLE_EMPTY);
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = (CookingTime)1000;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
    }

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = (Difficulty)1000;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
    }

    [Fact]
    public void Error_Ingredients_Empty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Clear();

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);
    }

    [Fact]
    public void Error_Instructions_Empty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.Clear();

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Empty_Value_Ingredients(string? ingredient)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Add(ingredient!);

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.INGREDIENT_EMPTY);
    }

    [Fact]
    public void Error_Same_Step_Instruction()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER);
    }

    [Fact]
    public void Error_Negative_Step_Instruction()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = -1;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.GREATER_THAN_ZERO_INSTRUCTION_STEP);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Empty_Value_Instruction(string? instruction)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = instruction!;

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.INSTRUCTION_EMPTY);
    }

    [Fact]
    public void Error_Instruction_Too_Long()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacteres: 2001);

        var result = new RecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS);
    }
}
