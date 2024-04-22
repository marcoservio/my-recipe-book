using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "user";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(METHOD, request, "tokenInvalid", culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Expired(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.TokenExpired();

        var response = await DoPut(METHOD, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Invalid(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(METHOD, request, string.Empty, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(METHOD, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
