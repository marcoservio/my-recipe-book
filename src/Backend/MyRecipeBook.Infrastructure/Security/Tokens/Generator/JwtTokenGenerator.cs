﻿using Microsoft.IdentityModel.Tokens;

using MyRecipeBook.Domain.Security.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Generator;

public class JwtTokenGenerator(uint expirationTimeInMinutes, string signingKey) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeInMinutes = expirationTimeInMinutes;
    private readonly string _signingKey = signingKey;

    public string Generate(Guid userIdentifier)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
