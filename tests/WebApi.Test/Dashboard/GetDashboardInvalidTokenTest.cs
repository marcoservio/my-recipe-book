using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Dashboard;

public class GetDashboardInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "dashboard";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet(method: METHOD, token: "tokenInvalid", culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Expired(string culture)
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.TokenExpired();

        var response = await DoGet(method: METHOD, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoGet(method: METHOD, token: string.Empty, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(method: METHOD, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
