using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.API.Authentication;

/// <summary>
/// Stub of OAuth 2.0 provider endpoints to enable test bearer tokens to be verified by ASP.NET Core Jwt Bearer middleware
/// during component testing without actually consuming the OAuth 2.0 provider endpoints.
/// Benefits:
/// 1. Component tests are more robust as they don't depend on OAuth 2.0 provider endpoints being available.
/// 2. Component tests are faster.
/// 3. When OAuth 2.0 provider public keys are rotated we don't need to regenerate the static bearer tokens we are using in component tests.
/// </summary>
public class StubJwtBearerAuthenticationHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (RequestedUrlIsOpenIdConfiguration(request))
        {
            return Task.FromResult(BuildOpenIdConfigurationResponse());
        }

        if (RequestedUrlIsJsonWebKeys(request))
        {
            return Task.FromResult(BuildJsonWebKeysResponse());
        }

        throw new NotImplementedException("Only openid-configuration and jwks.json urls are implemented in this stub.");
    }

    private static HttpResponseMessage BuildJsonWebKeysResponse()
    {
        // TODO - Copy response from https://{OAuth 2.0 provider domain}/.well-known/jwks.json
        return BuildSuccessfulJsonResponse("{ \"contents\": \"TODO\" }");
    }

    private static HttpResponseMessage BuildOpenIdConfigurationResponse()
    {
        // TODO - Copy response from https://{OAuth 2.0 provider domain}/.well-known/openid-configuration
        return BuildSuccessfulJsonResponse("{ \"jwks_uri\": \"https://oauth-provider.com/.well-known/jwks.json\" }");
    }

    private static HttpResponseMessage BuildSuccessfulJsonResponse(string json)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private static bool RequestedUrlIsJsonWebKeys(HttpRequestMessage request)
    {
        return request.RequestUri.AbsoluteUri.EndsWith("jwks.json");
    }

    private static bool RequestedUrlIsOpenIdConfiguration(HttpRequestMessage request)
    {
        return request.RequestUri.AbsoluteUri.EndsWith("openid-configuration");
    }
}
