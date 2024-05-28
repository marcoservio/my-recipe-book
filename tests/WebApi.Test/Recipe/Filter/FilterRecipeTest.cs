using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

using Xunit;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe/filter";

    private readonly Lazy<Guid> _userIdentifier = new(factory.GetUserIdentifier);

    private Lazy<string> _recipeTitle = new(factory.GetRecipeTitle);
    private Lazy<MyRecipeBook.Domain.Enums.Difficulty> _recipeDifficultyLevel = new(factory.GetRecipeDifficulty);
    private Lazy<MyRecipeBook.Domain.Enums.CookingTime> _recipeCookinTime = new(factory.GetRecipeCookingTime);
    private Lazy<IList<MyRecipeBook.Domain.Enums.DishType>> _recipeDishType = new(factory.GetDishType);

    [Fact]
    public async Task Success()
    {
        var request = new RequestFilterRecipeJson
        {
            CookingTimes = [(MyRecipeBook.Communication.Enums.CookingTime)_recipeCookinTime.Value],
            Difficulties = [(MyRecipeBook.Communication.Enums.Difficulty)_recipeDifficultyLevel.Value],
            DishTypes = _recipeDishType.Value.Select(dishType => (MyRecipeBook.Communication.Enums.DishType)dishType).ToList(),
            RecipeTitle_Ingredient = _recipeTitle.Value
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.RecipeTitle_Ingredient = "recipeDontExist";

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CookingTime_Invalid(string culture)
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
