﻿using CommonTestUtilities.Requests;

using FluentAssertions;

using MyRecipeBook.Exceptions;

using System.Globalization;
using System.Net;
using System.Text.Json;

using WebApi.Test.InlineData;

using Xunit;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private readonly string METHOD = "user";    

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()
            .Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;        

        var response = await DoPost(method: METHOD, request: request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
