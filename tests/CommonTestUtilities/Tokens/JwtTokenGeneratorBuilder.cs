using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        return new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "ttttttttttttttttttttttttttttttttttttttttttttttt");
    }

    public static string TokenExpired()
    {
        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiJhNWNiODBkMy1mOWY5LTRkZDAtYjE5NS1hMGZlZGU3NTU2ZjEiLCJuYmYiOjE3MTM1NjMyNjYsImV4cCI6MTcxMzU2MzU2NiwiaWF0IjoxNzEzNTYzMjY2fQ.iOF48cghPUrgFLZCqIt15JivgMTl8DRKSKziEmP1Fwk";
    }
}
