using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;

using WebApi.Test.InlineData;

using Xunit;

namespace WebApi.Test.Recipe.Update;

public class UpdateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "recipe";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: "tokenInvalid", culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Expired(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.TokenExpired();

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: string.Empty, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
