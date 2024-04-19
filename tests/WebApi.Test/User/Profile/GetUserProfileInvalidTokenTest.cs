using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;

using WebApi.Test.InlineData;

using Xunit;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "user";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var response = await DoGet(METHOD, "tokenInvalid", culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Expired(string culture)
    {
        var token = JwtTokenGeneratorBuilder.TokenExpired();

        var response = await DoGet(METHOD, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Invalid(string culture)
    {
        var response = await DoGet(METHOD, string.Empty, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(METHOD, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
