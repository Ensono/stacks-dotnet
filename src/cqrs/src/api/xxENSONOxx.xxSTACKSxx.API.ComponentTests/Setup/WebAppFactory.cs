using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

//TODO: Decide if we move this to a core package to be reused by multiple test projects

/// <summary>
/// WebAppFactory will be responsible for creating a TestServer and HttpClient handlers
/// </summary>
/// <typeparam name="TStartup">The API Startup class</typeparam>
public class WebAppFactory<TStartup>(
    Action<IServiceCollection> configureTestServices,
    Action<IWebHostBuilder> configureBuilder)
    : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        var builder = WebHost.CreateDefaultBuilder();

        configureBuilder(builder);

        builder.UseStartup<TStartup>();

        return builder;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(configureTestServices);
    }
}
