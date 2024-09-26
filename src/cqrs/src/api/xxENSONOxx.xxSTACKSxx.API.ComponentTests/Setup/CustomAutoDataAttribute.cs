using AutoFixture;
using AutoFixture.Xunit2;
using NSubstitute;
using xxENSONOxx.xxSTACKSxx.Abstractions.Commands;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

public class CustomAutoDataAttribute() : AutoDataAttribute(Customizations)
{
    private static IFixture Customizations()
    {
        var fixture = new Fixture();

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
