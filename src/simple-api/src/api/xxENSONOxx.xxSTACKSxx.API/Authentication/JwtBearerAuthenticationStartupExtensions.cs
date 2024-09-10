using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace xxENSONOxx.xxSTACKSxx.API.Authentication;

public static class JwtBearerAuthenticationStartupExtensions
{
    public static void AddJwtBearerTokenAuthentication(
        this IServiceCollection services,
        JwtBearerAuthenticationConfigurationExtension jwtBearerAuthenticationConfigurationExtension)
    {
        if (jwtBearerAuthenticationConfigurationExtension.IsDisabled())
        {
            return;
        }

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = jwtBearerAuthenticationConfigurationExtension.Authority;
                options.Audience = jwtBearerAuthenticationConfigurationExtension.Audience;

                if (jwtBearerAuthenticationConfigurationExtension.UseStubbedBackchannelHandler)
                {
                    options.BackchannelHttpHandler = new StubJwtBearerAuthenticationHttpMessageHandler();
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = !jwtBearerAuthenticationConfigurationExtension.AllowExpiredTokens
                };
            });
    }
}
