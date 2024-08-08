using Amido.Stacks.Testing.Settings;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
using xxAMIDOxx.xxSTACKSxx.API.Authentication;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests;

public class CustomAutoDataAttribute : AutoDataAttribute
{
    public CustomAutoDataAttribute() : base(Customizations)
    {
    }

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

        fixture.Register<IOptions<JwtBearerAuthenticationConfiguration>>(() => jwtBearerAuthenticationConfiguration.AsOption());

        return fixture;
    }
}
