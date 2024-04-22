using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using MyRecipeBook.Exceptions;

using System.Globalization;
using System.Net;
using System.Text.Json;

using WebApi.Test.InlineData;

using Xunit;

namespace WebApi.Test.User.Update;

public class UpdateUserTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "user";

    private readonly Lazy<Guid> _userIdentifier = new(factory.GetUserIdentifier);

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPut(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPut(METHOD, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
