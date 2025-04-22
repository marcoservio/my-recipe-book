using CommonTestUtilities.ErrorMessage;

using FluentAssertions;

using MyRecipeBook.Communication.Requests;

using System.Net;
using System.Text.Json;

using WebApi.Test.InlineData;

namespace WebApi.Test.Token.RefreshToken;

public class UserRefreshTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "token/refresh-token";

    [Fact]
    public async Task Success()
    {
        var request = new RequestNewTokenJson
        {
            RefreshToken = _refreshToken
        };

        var response = await DoPost(method: METHOD, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound_RefrehToken(string culture)
    {
        var request = new RequestNewTokenJson
        {
            RefreshToken = "invalidRefreshToken"
        };

        var response = await DoPost(method: METHOD, request: request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "INVALID_SESSION");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
