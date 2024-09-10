using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Authorization;

namespace xxENSONOxx.xxSTACKSxx.API.UnitTests;

public class ConfigurableAuthorizationPolicyProviderTests
{
    [Fact]
    public async Task GetDefaultPolicyAsync_ReturnsDefaultPolicy_WhenJwtBearerAuthenticationIsEnabled()
    {
        var options = Options.Create(new AuthorizationOptions());
        var jwtOptions = Options.Create(new JwtBearerAuthenticationConfigurationExtension { Enabled = false });
        var provider = new ConfigurableAuthorizationPolicyProvider(options, jwtOptions);

        var result = await provider.GetDefaultPolicyAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Requirements);
    }
}
