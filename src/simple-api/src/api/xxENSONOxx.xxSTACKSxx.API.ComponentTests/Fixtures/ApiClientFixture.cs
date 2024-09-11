using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

/// <summary>
/// ApiClientFixture handles the communication with TestServer
/// and httpClient handling
/// </summary>
public abstract class ApiClientFixture(IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    : ApiFixture<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"JwtBearerAuthentication:AllowExpiredTokens", jwtBearerAuthenticationOptions.Value.AllowExpiredTokens.ToString().ToLowerInvariant()},
                {"JwtBearerAuthentication:Audience", jwtBearerAuthenticationOptions.Value.Audience},
                {"JwtBearerAuthentication:Authority", jwtBearerAuthenticationOptions.Value.Authority},
                {"JwtBearerAuthentication:Enabled", jwtBearerAuthenticationOptions.Value.Enabled.ToString().ToLowerInvariant()},
                {"JwtBearerAuthentication:OpenApi:AuthorizationUrl", jwtBearerAuthenticationOptions.Value.OpenApi?.AuthorizationUrl!},
                {"JwtBearerAuthentication:OpenApi:ClientId", jwtBearerAuthenticationOptions.Value.OpenApi?.ClientId!},
                {"JwtBearerAuthentication:OpenApi:TokenUrl", jwtBearerAuthenticationOptions.Value.OpenApi?.TokenUrl!},
                {"JwtBearerAuthentication:UseStubbedBackchannelHandler", jwtBearerAuthenticationOptions.Value.UseStubbedBackchannelHandler.ToString().ToLowerInvariant()},
            }!);
        });
    }

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        collection.AddSingleton<IOptions<JwtBearerAuthenticationConfiguration>>(_ => jwtBearerAuthenticationOptions);
    }

    /// <summary>
    /// Send a POST Http request to the API CreateMenu endpoint passing the menu being created
    /// </summary>
    /// <param name="menu">Menu being created</param>
    public async Task<HttpResponseMessage> CreateMenu(CreateMenuRequest menu)
    {
        return await SendAsync(HttpMethod.Post, "/v1/menu", menu);
    }

    internal void ThenASuccessfulResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

    internal void ThenACreatedResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}
