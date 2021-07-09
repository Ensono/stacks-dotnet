using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public static class JwtBearerAuthenticationStartupExtensions
    {
        public static void AddJwtBearerTokenAuthentication(
            this IServiceCollection services,
            JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration)
        {
            if (jwtBearerAuthenticationConfiguration.IsDisabled())
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
                    options.Authority = jwtBearerAuthenticationConfiguration.Authority;
                    options.Audience = jwtBearerAuthenticationConfiguration.Audience;

                    if (jwtBearerAuthenticationConfiguration.UseStubbedBackchannelHandler)
                    {
                        options.BackchannelHttpHandler = new StubJwtBearerAuthenticationHttpMessageHandler();
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = !jwtBearerAuthenticationConfiguration.AllowExpiredTokens
                    };
                });
        }
    }
}
