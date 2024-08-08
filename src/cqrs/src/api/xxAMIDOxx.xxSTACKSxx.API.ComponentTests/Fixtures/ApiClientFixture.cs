using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using xxAMIDOxx.xxSTACKSxx.API.Authentication;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.API.Models.Responses;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests.Fixtures;

/// <summary>
/// ApiClientFixture handles the communication with TestServer
/// and httpClient handling
/// </summary>
public abstract class ApiClientFixture : ApiFixture<Startup>
{
    private readonly IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions;

    protected ApiClientFixture(
        IOptions<JwtBearerAuthenticationConfiguration> jwtBearerAuthenticationOptions)
    {
        this.jwtBearerAuthenticationOptions = jwtBearerAuthenticationOptions;
    }

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
                {"JwtBearerAuthentication:OpenApi:AuthorizationUrl", jwtBearerAuthenticationOptions.Value.OpenApi?.AuthorizationUrl},
                {"JwtBearerAuthentication:OpenApi:ClientId", jwtBearerAuthenticationOptions.Value.OpenApi?.ClientId},
                {"JwtBearerAuthentication:OpenApi:TokenUrl", jwtBearerAuthenticationOptions.Value.OpenApi?.TokenUrl},
                {"JwtBearerAuthentication:UseStubbedBackchannelHandler", jwtBearerAuthenticationOptions.Value.UseStubbedBackchannelHandler.ToString().ToLowerInvariant()},
            });
        });
    }

    protected override void RegisterDependencies(IServiceCollection collection)
    {
        DependencyRegistration.ConfigureStaticDependencies(collection);

        collection.AddSingleton<IOptions<JwtBearerAuthenticationConfiguration>>(serviceProvider => jwtBearerAuthenticationOptions);
    }

    /// <summary>
    /// Adds bearer token to the request based on role name.
    /// When a feature has same behaviour for multiple roles
    /// We could use the same theory and test multiple roles
    /// </summary>
    /// <param name="role">Name of role being authenticated</param>
    public string GivenTheUserIsAuthenticatedAndHasRole(string role)
    {
        switch (role.ToLower())
        {
            case "admin":
                return GivenTheUserIsAnAuthenticatedAdministrator();
            case "employee":
                return GivenTheUserIsAnAuthenticatedEmployee();
            case "customer":
                return GivenTheUserIsAnAuthenticatedCustomer();
            default:
                return GivenTheUserIsUnauthenticated();
        }
    }

    /// <summary>
    /// Adds an Admin bearer token to the request
    /// </summary>
    public string GivenTheUserIsAnAuthenticatedAdministrator() => HttpClient.AsAdmin();

    /// <summary>
    /// Adds an Employee bearer token to the request
    /// </summary>
    public string GivenTheUserIsAnAuthenticatedEmployee() => HttpClient.AsEmployee();

    /// <summary>
    /// Adds a Customer bearer token to the request
    /// </summary>
    public string GivenTheUserIsAnAuthenticatedCustomer() => HttpClient.AsCustomer();

    /// <summary>
    /// Removes any bearer token from the request to simulate unauthenticated user
    /// </summary>
    public string GivenTheUserIsUnauthenticated() => HttpClient.AsUnauthenticatedUser();

    /// <summary>
    /// Send a POST Http request to the API CreateMenu endpoint passing the menu being created
    /// </summary>
    /// <param name="menu">Menu being created</param>
    public async Task<HttpResponseMessage> CreateMenu(CreateMenuRequest menu)
    {
        return await SendAsync(HttpMethod.Post, "/v1/menu", menu);
    }

    /// <summary>
    /// Send a POST Http request to the API CreateCategory endpoint passing the menu id and category being created
    /// </summary>
    /// <param name="category">Category being created</param>
    public async Task<HttpResponseMessage> CreateCategory(Guid menuId, CreateCategoryRequest category)
    {
        return await SendAsync(HttpMethod.Post, $"/v1/menu/{menuId}/category", category);
    }

    internal void ThenASuccessfulResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeTrue();
    }

    internal void ThenAFailureResponseIsReturned()
    {
        LastResponse.IsSuccessStatusCode.ShouldBeFalse();
    }

    internal void ThenAForbiddenResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    internal void ThenACreatedResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    internal void ThenANotFoundResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    internal void ThenAConflictResponseIsReturned()
    {
        LastResponse.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    internal async Task ThenTheResourceCreatedResponseIsReturned()
    {
        var responseObject = await GetResponseObject<ResourceCreatedResponse>();

        responseObject.ShouldNotBeNull();
        responseObject.Id.ShouldNotBe(default(Guid));
    }
}
