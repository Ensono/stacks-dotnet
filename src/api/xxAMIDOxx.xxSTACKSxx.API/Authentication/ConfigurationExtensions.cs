using Microsoft.Extensions.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationSection GetJwtBearerAuthenticationConfigurationSection(this IConfiguration configuration) =>
            configuration.GetSection("JwtBearerAuthentication");
    }
}
