using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using xxENSONOxx.xxSTACKSxx.API.Authentication;

namespace xxENSONOxx.xxSTACKSxx.API.Authorization;

public class ConfigurableAuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> options,
    IOptions<JwtBearerAuthenticationConfigurationExtension> jwtBearerAuthenticationOptions)
    : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider fallbackPolicyProvider = new(options);

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
