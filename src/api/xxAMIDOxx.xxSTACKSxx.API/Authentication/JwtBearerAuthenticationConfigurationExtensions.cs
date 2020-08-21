namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public static class JwtBearerAuthenticationConfigurationExtensions
    {
        public static bool HasOpenApiClient(this JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration) =>
            jwtBearerAuthenticationConfiguration.IsEnabled() &&
            jwtBearerAuthenticationConfiguration.OpenApi != null;

        public static bool IsDisabled(this JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration) =>
            !jwtBearerAuthenticationConfiguration.IsEnabled();

        public static bool IsEnabled(this JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration) =>
            jwtBearerAuthenticationConfiguration != null && jwtBearerAuthenticationConfiguration.Enabled;
    }
}
