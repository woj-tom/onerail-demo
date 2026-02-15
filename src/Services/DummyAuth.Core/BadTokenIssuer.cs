using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DummyAuth.Core;

public class BadTokenIssuer
{
    public string GetToken(
        string keyStr,
        string issuer,
        string audience)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "read"),
            new Claim(ClaimTypes.Role, "write")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(keyStr
                                   ?? throw new InvalidOperationException("Service misconfigured"))
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            // This needs to be configured per service
            audience: audience,
            claims: claims,
            // This is a VERY long living token
            expires: DateTime.UtcNow.AddYears(100),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}