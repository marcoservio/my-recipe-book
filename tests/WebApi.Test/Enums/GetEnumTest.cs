using CommonTestUtilities.ErrorMessage;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;
using System.Reflection.Emit;
using System.Text.Json;

using WebApi.Test.InlineData;

namespace WebApi.Test.Enums;

public class GetEnumTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "enums";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(method: $"{METHOD}/{_enumName}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("enums").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Enum_Not_Found(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(method: $"{METHOD}/{_enumNotFoundName}", token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "ENUM_NOT_FOUND");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
