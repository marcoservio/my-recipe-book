using System.Diagnostics.CodeAnalysis;

namespace MyRecipeBook.Domain.Extensions;

public static class StringExtension
{
    public static bool NotEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrWhiteSpace(value);
    public static bool Empty([NotNullWhen(true)] this string value) => string.IsNullOrWhiteSpace(value);
}
