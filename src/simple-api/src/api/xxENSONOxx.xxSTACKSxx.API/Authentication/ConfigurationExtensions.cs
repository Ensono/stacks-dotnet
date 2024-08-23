using Microsoft.Extensions.Configuration;

namespace xxENSONOxx.xxSTACKSxx.API.Authentication;

public static class ConfigurationExtensions
{
    public static IConfigurationSection GetJwtBearerAuthenticationConfigurationSection(this IConfiguration configuration) =>
        configuration.GetSection("JwtBearerAuthentication");
}
