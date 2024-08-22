using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests;

public class TestWebApplicationFactory(Action<IServiceCollection> registerDependencies) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(registerDependencies);
        return base.CreateHost(builder);
    }
}
