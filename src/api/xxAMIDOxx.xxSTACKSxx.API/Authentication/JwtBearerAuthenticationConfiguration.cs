namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public class JwtBearerAuthenticationConfiguration
    {
        public bool AllowExpiredTokens { get; set; }

        public string Audience { get; set; }

        public string Authority { get; set; }

        public bool Enabled { get; set; } = true;

        public string OpenApiAuthorizationUrl { get; set; }

        public string OpenApiClientId { get; set; }

        public bool UseStubbedBackchannelHandler { get; set; }
    }
}
