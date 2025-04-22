using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;
using System.Text.Json;

namespace WebApi.Test.Dashboard;

public class GetDashboardTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "dashboard";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(method: METHOD, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        await DoDelete(method: $"recipes/{_recipeId}", token: token);

        var response = await DoGet(method: METHOD, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
