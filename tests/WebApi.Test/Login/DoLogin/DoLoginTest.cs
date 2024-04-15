using MyRecipeBook.Communication.Requests;

using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

using WebApi.Test.InlineData;

using Xunit;
using FluentAssertions;
using CommonTestUtilities.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string _method = "login";
    private readonly HttpClient _httpClient = factory.CreateClient();
    private readonly string _email = factory.GetEmail();
    private readonly string _password = factory.GetPassword();
    private readonly string _name = factory.GetName();

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await _httpClient.PostAsJsonAsync(_method, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(_name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalido(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);

        var response = await _httpClient.PostAsJsonAsync(_method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}

