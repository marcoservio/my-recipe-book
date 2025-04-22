using CommonTestUtilities.ErrorMessage;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;

using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete;

public class DeleteRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "recipes";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoDelete(method: $"{METHOD}/{_recipeId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(method: $"{METHOD}/{_recipeId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_Not_Found(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var id = IdEncripterBuilder.Build().Encode(1000);

        var response = await DoDelete(method: $"{METHOD}/{id}", token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "RECIPE_NOT_FOUND");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
