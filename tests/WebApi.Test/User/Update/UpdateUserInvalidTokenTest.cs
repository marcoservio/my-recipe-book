using CommonTestUtilities.ErrorMessage;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "users";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, token: "invalid_token", culture: culture);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "USER_WITHOUT_PERMISSION_ACCESS_RESOURCE");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, token: string.Empty, culture: culture);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "NO_TOKEN");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Expired(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.TokenExpired();

        var response = await DoPut(method: METHOD, request: request, token: token, culture: culture);

        (var expectedMessage, var errors, var tokenIsExpired) = await ErrorHandler.GetErrorMessage(response, culture, "TOKEN_EXPIRED");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        tokenIsExpired.Should().BeTrue();

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(method: METHOD, request: request, token: token, culture: culture);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "USER_WITHOUT_PERMISSION_ACCESS_RESOURCE");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
