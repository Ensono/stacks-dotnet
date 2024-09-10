namespace xxENSONOxx.xxSTACKSxx.API.Authentication;

public static class JwtBearerAuthenticationConfigurationExtensions
{
    public static bool HasOpenApiClient(this JwtBearerAuthenticationConfigurationExtension jwtBearerAuthenticationConfigurationExtension) =>
        jwtBearerAuthenticationConfigurationExtension.IsEnabled() &&
        jwtBearerAuthenticationConfigurationExtension.OpenApi != null;

    public static bool IsDisabled(this JwtBearerAuthenticationConfigurationExtension jwtBearerAuthenticationConfigurationExtension) =>
        !jwtBearerAuthenticationConfigurationExtension.IsEnabled();

    public static bool IsEnabled(this JwtBearerAuthenticationConfigurationExtension jwtBearerAuthenticationConfigurationExtension) =>
        jwtBearerAuthenticationConfigurationExtension != null && jwtBearerAuthenticationConfigurationExtension.Enabled;
}
