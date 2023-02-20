using System;
using System.Security.Claims;
using System.Text;
using BitPastry.Backend.DTO.Configurations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace BitPastry.Backend.Presentation.Auth;

public class TokensProvider
{
    private string secret;
    private readonly int MITUTES;

    public TokensProvider(JWTConfiguration conf)
    {
        secret = conf.Secret;
        MITUTES = conf.TokenValidityMinutes;
    }

    public string ProvideToken(long id, string username, bool isRememberMe = false)
    {
        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, username)
            }),
            Audience = "http://localhost:3000/",
            Expires = isRememberMe ? DateTime.UtcNow.AddYears(1) : DateTime.UtcNow.AddMinutes(MITUTES),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256Signature)
        });

        return handler.WriteToken(token);
    }
}

