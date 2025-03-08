using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Onefocus.Common.Configurations;
using Onefocus.Common.Security;
using System.Text;

namespace Onefocus.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationSettings(
        this IServiceCollection services
        , IConfiguration configuration
        , bool includeAuthentication = true)
    {
        var authSettings = configuration.GetSection(IAuthenticationSettings.SettingName).Get<AuthenticationSettings>()!;

        services.AddSingleton<IAuthenticationSettings>(authSettings);

        if (includeAuthentication)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = authSettings.Audience,
                    ValidIssuer = authSettings.Issuer,
                    IssuerSigningKey = Cryptography.CreateSymmetricSecurityKey(authSettings.SymmetricSecurityKey),
                    ClockSkew = TimeSpan.FromSeconds(2),
                    RequireExpirationTime = true
                };
            });
        }

        return services;
    }
}

