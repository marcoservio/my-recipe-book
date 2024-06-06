using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;
using System.Text.Json;

using Xunit;

namespace WebApi.Test.Dashboard;

public class GetDashboardTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "dashboard";

    private readonly Lazy<Guid> _userIdentifier = new(factory.GetUserIdentifier);

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoGet(method: METHOD, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").GetArrayLength().Should().BeGreaterThan(0);
    }
}
