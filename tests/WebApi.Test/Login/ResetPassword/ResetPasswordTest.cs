using CommonTestUtilities.ErrorMessage;
using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Communication.Requests;

using System.Net;

using WebApi.Test.InlineData;

namespace WebApi.Test.Login.ResetPassword;
public class ResetPasswordTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "login/reset-password";

    [Fact]
    public async Task Success()
    {
        var request = RequestResetYourPasswordJsonBuilder.Build(email: _email, code: _resetPasswordCode);

        var response = await DoPut(method: METHOD, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        response = await DoPost(method: "login", request: loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost(method: "login", request: loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Code_Not_Found(string culture)
    {
        var request = RequestResetYourPasswordJsonBuilder.Build();

        var response = await DoPut(method: METHOD, request: request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        (var expectedMessage, var errors, _) = await ErrorHandler.GetErrorMessage(response, culture, "CODE_IS_NULL");

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(expectedMessage));
    }
}
