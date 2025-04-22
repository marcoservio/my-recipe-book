using CommonTestUtilities.ErrorMessage;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Update;
public class UpdateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "recipes";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Invalid(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build(); 
        
        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: "invalid_token", culture: culture);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "USER_WITHOUT_PERMISSION_ACCESS_RESOURCE");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: string.Empty, culture: culture);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "NO_TOKEN");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Expired(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.TokenExpired();

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: token, culture: culture);

        (var expectedMessage, var errors, var tokenIsExpired) = await ErrorHandler.GetErrorMessage(response, culture, "TOKEN_EXPIRED");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        tokenIsExpired.Should().BeTrue();

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_With_User_NotFound(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut(method: $"{METHOD}/{id}", request: request, token: token, culture: culture);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "USER_WITHOUT_PERMISSION_ACCESS_RESOURCE");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
