using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Authentication;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests.JwtBearerAuthentication;

public class JwtBearerAuthenticationConfigurationExtensionTests
{
    [Fact]
    public void HasOpenApiClient_ReturnsTrue_WhenEnabledAndOpenApiIsNotNull()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = true, OpenApi = new OpenApiJwtBearerAuthenticationConfiguration() };

        var result = config.HasOpenApiClient();

        Assert.True(result);
    }

    [Fact]
    public void HasOpenApiClient_ReturnsFalse_WhenDisabled()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = false, OpenApi = new OpenApiJwtBearerAuthenticationConfiguration() };

        var result = config.HasOpenApiClient();

        Assert.False(result);
    }

    [Fact]
    public void HasOpenApiClient_ReturnsFalse_WhenOpenApiIsNull()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = true, OpenApi = null };

        var result = config.HasOpenApiClient();

        Assert.False(result);
    }

    [Fact]
    public void IsDisabled_ReturnsTrue_WhenDisabled()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = false };

        var result = config.IsDisabled();

        Assert.True(result);
    }

    [Fact]
    public void IsDisabled_ReturnsFalse_WhenEnabled()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = true };

        var result = config.IsDisabled();

        Assert.False(result);
    }

    [Fact]
    public void IsEnabled_ReturnsTrue_WhenEnabled()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = true };

        var result = config.IsEnabled();

        Assert.True(result);
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenDisabled()
    {
        var config = new JwtBearerAuthenticationConfigurationExtension { Enabled = false };

        var result = config.IsEnabled();

        Assert.False(result);
    }
}
