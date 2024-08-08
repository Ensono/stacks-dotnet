using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using xxAMIDOxx.xxSTACKSxx.API.Authentication;

namespace xxAMIDOxx.xxSTACKSxx.API.Authorization;

public class ConfigurableAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions;
    private readonly DefaultAuthorizationPolicyProvider fallbackPolicyProvider;

    public ConfigurableAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options,
        IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    {
        this.jwtBearerAuthenticationOptions = jwtBearerAuthenticationOptions;
        fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        BuildAuthorizationPolicy(() => fallbackPolicyProvider.GetDefaultPolicyAsync());

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() =>
        fallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName) =>
        BuildAuthorizationPolicy(() => fallbackPolicyProvider.GetPolicyAsync(policyName));

    private Task<AuthorizationPolicy> BuildAuthorizationPolicy(Func<Task<AuthorizationPolicy>> authorizationPolicyFactory)
    {
        // If JWT bearer authentication is disabled just ignore the [Authorize] attribute and authorize the request.
        // If JWT bearer authentication is enabled authorize the request using the provided policy
        return
            jwtBearerAuthenticationOptions.Value.IsDisabled()
                ? Task.FromResult(new AuthorizationPolicyBuilder().RequireAssertion(handler => true).Build())
                : authorizationPolicyFactory();
    }
}
