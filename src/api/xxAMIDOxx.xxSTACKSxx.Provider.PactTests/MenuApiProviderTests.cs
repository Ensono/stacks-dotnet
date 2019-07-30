using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.Core;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;
using xxAMIDOxx.xxSTACKSxx.API;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxAMIDOxx.xxSTACKSxx.Provider.PactTests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Provider.PactTests
{
    public class MenuApiProviderTests : IDisposable
    {
        private string ProviderUri { get; }

        private string PactServiceUri { get; }

        private IWebHost ProviderStateHost { get; }
        private IWebHost ProviderWebHost { get; }

        private ITestOutputHelper OutputHelper { get; }

        public MenuApiProviderTests(ITestOutputHelper output)
        {
            OutputHelper = output;

            PactServiceUri = "http://localhost:9001";

            ProviderStateHost = WebHost.CreateDefaultBuilder()
                .UseUrls(PactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            ProviderStateHost.Start();

            ProviderUri = "http://localhost:6001";
            ProviderWebHost = WebHost.CreateDefaultBuilder()
                .UseUrls(ProviderUri)
                .UseStartup<Startup>()
                .ConfigureServices(ConfigureDependencies)
                .Build();

            ProviderWebHost.Start();
        }


        private void ConfigureDependencies(IServiceCollection services)
        {
            var getMenuIdQueryCriteria = Substitute.For<IQueryHandler<GetMenuByIdQueryCriteria, Menu>>();

            getMenuIdQueryCriteria.ExecuteAsync(Arg.Any<GetMenuByIdQueryCriteria>()).Returns(FakeResponse);

            services.AddTransient(x => getMenuIdQueryCriteria);
        }

        private Task<Menu> FakeResponse(CallInfo arg)
        {
            var menu = new Menu
            {
                Id = Guid.Parse("9dbffe96-daa5-4adc-a888-34e41dc205d4"),
                Name = "menu tuga",
                Description = "pastel de nata",
                Enabled = true,
                Categories = null
            };

            return Task.FromResult(menu);
        }

        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer()
        {
            // Arrange
            var config = new PactVerifierConfig
            {

                // NOTE: We default to using a ConsoleOutput,
                // however xUnit 2 does not capture the console output,
                // so a custom outputter is required.
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(OutputHelper)
                },

                // Output verbose verification logs to the test output
                Verbose = true
            };

            //Act / Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{PactServiceUri}/provider-states")
                .ServiceProvider("MenuAPI", ProviderUri)
                .HonoursPactWith("GenericMenuConsumer")
                .PactUri(@"..\..\..\..\pacts\genericmenuconsumer-menuapi.json")
                .Verify();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ProviderStateHost.StopAsync().GetAwaiter().GetResult();
                    ProviderStateHost.Dispose();

                    ProviderWebHost.StopAsync().GetAwaiter().GetResult();
                    ProviderWebHost.Dispose();
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
