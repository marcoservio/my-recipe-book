using CommonTestUtilities.ErrorMessage;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using System.Net;

using WebApi.Test.InlineData;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
    {
        private readonly string METHOD = "users";

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Invalid(string culture)
        {
            var response = await DoGet(method: METHOD, token: "invalid_token", culture: culture);

            (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "USER_WITHOUT_PERMISSION_ACCESS_RESOURCE");

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            var response = await DoGet(method: METHOD, token: string.Empty, culture: culture);

            (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "NO_TOKEN");

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Expired(string culture)
        {
            var token = JwtTokenGeneratorBuilder.TokenExpired();

            var response = await DoGet(method: METHOD, token: token, culture: culture);

            (var expectedMessage, var errors, var tokenIsExpired) = await ErrorHandler.GetErrorMessage(response, culture, "TOKEN_EXPIRED");

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
            
            tokenIsExpired.Should().BeTrue();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_With_User_NotFound(string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(method: METHOD, token: token, culture: culture);

            (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "USER_WITHOUT_PERMISSION_ACCESS_RESOURCE");

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
