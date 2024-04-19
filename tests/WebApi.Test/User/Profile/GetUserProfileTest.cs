using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;
using System.Text.Json;

using Xunit;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "user";

    private readonly Lazy<string> _name = new(factory.GetName);
    private readonly Lazy<string> _email = new(factory.GetEmail);
    private readonly Lazy<Guid> _userIdentifier = new(factory.GetUserIdentifier);

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoGet(METHOD, token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name.Value);
        responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email.Value);
    }
}
