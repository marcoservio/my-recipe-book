﻿using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;

using FluentAssertions;

using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Recipe.Update;

public class UpdateRecipeTest(CustomWebApplicationFactory factory) : MyRecipeBookClassFixture(factory)
{
    private const string METHOD = "recipe";

    private readonly Lazy<Guid> _userIdentifier = new(factory.GetUserIdentifier);
    private readonly Lazy<string> _recipeId = new(factory.GetRecipeId);

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPut(method: $"{METHOD}/{_recipeId.Value}", request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier.Value);

        var response = await DoPut(method: $"{METHOD}/{_recipeId.Value}", request: request, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
