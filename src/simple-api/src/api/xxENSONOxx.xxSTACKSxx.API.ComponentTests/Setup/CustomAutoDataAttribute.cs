using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

public class CustomAutoDataAttribute() : AutoDataAttribute(Customizations)
{
    public static IFixture Customizations()
    {
        var fixture = new Fixture();

        // TODO - Set JWT authentication config settings if enabled
        var jwtBearerAuthenticationConfiguration = new JwtBearerAuthenticationConfiguration
        {
            AllowExpiredTokens = true,
            Audience = "<TODO>",
            Authority = "<TODO>",
            Enabled = false,
            OpenApi = new OpenApiJwtBearerAuthenticationConfiguration
            {
                AuthorizationUrl = "<TODO>",
                ClientId = "<TODO>",
                TokenUrl = "<TODO>"
            },
            UseStubbedBackchannelHandler = true
        };

        fixture.Register(() => jwtBearerAuthenticationConfiguration.AsOption());

        return fixture;
    }
}
