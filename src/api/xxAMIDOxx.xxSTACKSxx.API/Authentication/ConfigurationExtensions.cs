using Microsoft.Extensions.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public static class ConfigurationExtensions
    {
        public static JwtBearerAuthenticationConfiguration GetJwtBearerAuthenticationConfiguration(
            this IConfiguration configuration) =>
                configuration.GetSection("JwtBearerAuthentication").Get<JwtBearerAuthenticationConfiguration>();
    }
}
