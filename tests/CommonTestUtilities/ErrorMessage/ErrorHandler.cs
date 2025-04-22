using MyRecipeBook.Exceptions;

using System.Globalization;
using System.Text.Json;

using static System.Text.Json.JsonElement;

namespace CommonTestUtilities.ErrorMessage;

public class ErrorHandler
{
    public static async Task<(string, ArrayEnumerator, bool)> GetErrorMessage(HttpResponseMessage response, string culture, string message)
    {
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var tokenIsExpired = responseData.RootElement.GetProperty("tokenIsExpired").GetBoolean();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString(message, new CultureInfo(culture));

        return (expectedMessage!, errors, tokenIsExpired);
    }
}
