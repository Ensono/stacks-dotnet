using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace xxAMIDOxx.xxSTACKSxx.API.ComponentTests
{
    //TODO: Decide if we move this to a core package to be reused by multiple test projects

    /// <summary>
    /// WebAppFactory will be responsible for creating a TestServer and HttpClient handlers
    /// </summary>
    /// <typeparam name="TStartup">The API Startup class</typeparam>
    public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly Action<IServiceCollection> configureTestServices;
        private readonly Action<IWebHostBuilder> configureBuilder;

        public WebAppFactory(Action<IServiceCollection> configureTestServices, Action<IWebHostBuilder> configureBuilder)
        {
            this.configureTestServices = configureTestServices;
            this.configureBuilder = configureBuilder;
        }

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
}
