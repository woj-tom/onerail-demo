using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Utils.Extensions;

public static class AuthExtension
{
    public static IServiceCollection AddCustomAuth(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = config["JwtDummy:Issuer"],
                    ValidAudience = config["JwtDummy:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtDummy:Key"]
                                               ?? throw new InvalidOperationException("Service misconfigured"))
                    ),
                    
                    RoleClaimType = ClaimTypes.Role
                };
            });
        return services;
    }
}