namespace xxAMIDOxx.xxSTACKSxx.API.Authentication;

public class JwtBearerAuthenticationConfiguration
{
    public bool AllowExpiredTokens { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public bool Enabled { get; set; }

    public OpenApiJwtBearerAuthenticationConfiguration OpenApi { get; set; }

    public bool UseStubbedBackchannelHandler { get; set; }
}
