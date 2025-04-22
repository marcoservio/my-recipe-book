using FluentAssertions;

using System.Net;

namespace WebApi.Test.Login.RequestCode;

public class RequestCodeResetPasswordTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "login/code-reset-password";

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(method: $"{METHOD}/{_email}");

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
}
