using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Security.Tokens.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeInMinutes: 5, signingKey: "ttttttttttttttttttttttttttttttttttt");

    public static string TokenExpired()
    {
        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiJiODdlNTMxYy1hODliLTQ5NWItODMxZS1lMzQ4YmRiNTk4N2IiLCJuYmYiOjE3NDIzMzI5MzcsImV4cCI6MTc0MjMzMzIzNywiaWF0IjoxNzQyMzMyOTM3fQ.ZmhXJWB8DByC5CnDdg-E-9VzZUIrx1Yv9n7rLOxelxg";
    }
}
