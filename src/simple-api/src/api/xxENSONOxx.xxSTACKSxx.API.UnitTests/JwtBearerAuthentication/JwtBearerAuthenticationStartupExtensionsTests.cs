using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Authentication;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.JwtBearerAuthentication;

public class JwtBearerAuthenticationStartupExtensionsTests
{
    [Fact]
    public void AddJwtBearerTokenAuthentication_DoesNotAddAuthentication_WhenConfigurationIsDisabled()
    {
        var services = new ServiceCollection();
        var configuration = new JwtBearerAuthenticationConfigurationExtension { Enabled = false };

        services.AddJwtBearerTokenAuthentication(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var authenticationService = serviceProvider.GetService<IAuthenticationService>();

        Assert.Null(authenticationService);
    }

    [Fact]
    public void AddJwtBearerTokenAuthentication_AddsAuthentication_WhenConfigurationIsEnabled()
    {
        var services = new ServiceCollection();
        var configuration = new JwtBearerAuthenticationConfigurationExtension { Enabled = true };

        services.AddJwtBearerTokenAuthentication(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var authenticationService = serviceProvider.GetService<IAuthenticationService>();

        Assert.NotNull(authenticationService);
    }

    [Fact]
    public void AddJwtBearerTokenAuthentication_ValidatesLifetime_WhenAllowExpiredTokensIsFalse()
    {
        var services = new ServiceCollection();
        var configuration = new JwtBearerAuthenticationConfigurationExtension
        {
            Enabled = true, AllowExpiredTokens = false
        };

        services.AddJwtBearerTokenAuthentication(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<JwtBearerOptions>>().Value;

        Assert.True(options.TokenValidationParameters.ValidateLifetime);
    }
}
