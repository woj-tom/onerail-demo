using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();
 
// It doesn't make any sense
// This should be managed by IdP with asymmetric key
app.MapGet("/token", (IConfiguration config) =>
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, "read"),
        new Claim(ClaimTypes.Role, "write")
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(config["JwtDummy:Key"] 
                               ?? throw new InvalidOperationException("Service misconfigured"))
    );

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: config["JwtDummy:Issuer"],
        // This needs to be configured per service
        audience: config["JwtDummy:Audience"],
        claims: claims,
        // This is a VERY long living token
        expires: DateTime.UtcNow.AddYears(100),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
});

app.Run();
