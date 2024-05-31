using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Recipe.Delete;

public class DeleteRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe";

    private readonly Lazy<Guid> _userIdentifier = new(factory.GetUserIdentifier);
    private readonly Lazy<string> _recipeId = new(factory.GetRecipeId);

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoDelete(method: $"{METHOD}/{_recipeId.Value}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(method: $"{METHOD}/{_recipeId.Value}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_Not_Found(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var id = IdEncripterBuilder.Build().Encode(1000);

        var response = await DoDelete(method: $"{METHOD}/{id}", token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
