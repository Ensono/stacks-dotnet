using Microsoft.Extensions.Configuration;

namespace Snyk.Fixes.API.Authentication
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationSection GetJwtBearerAuthenticationConfigurationSection(this IConfiguration configuration) =>
            configuration.GetSection("JwtBearerAuthentication");
    }
}
