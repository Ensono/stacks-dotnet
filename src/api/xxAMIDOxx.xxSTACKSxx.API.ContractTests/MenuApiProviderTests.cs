using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;
using xxAMIDOxx.xxSTACKSxx.Provider.PactTests.Fixtures;

namespace xxAMIDOxx.xxSTACKSxx.Provider.PactTests
{
    public class MenuApiProviderTests : IDisposable
    {
        private string ProviderUri { get; }

        private IWebHost ProviderWebHost { get; }

        private ITestOutputHelper OutputHelper { get; }

        private readonly ConfigModel Config;

        public MenuApiProviderTests(ITestOutputHelper output)
        {
            OutputHelper = output;          
            
            //Create mock API service for the provider's API
            ProviderUri = "http://localhost:6001";
            ProviderWebHost = WebHost.CreateDefaultBuilder()
                .UseUrls(ProviderUri)
                .UseStartup<TestStartup>()
                .ConfigureServices(DependencyRegistration.ConfigureStaticServices)
                .Build();

            ProviderWebHost.Start();

            //Get application configuration
            Config = ConfigurationAccessor.GetApplicationConfiguration();
        }


        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer()
        {
            //This is the build number for the PROVIDER, not the consumer or broker.
            var buildNumber = Config.Build_Number;

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
                Verbose = true,

                //If build number is present, results will be published back to the broker
                ProviderVersion = !string.IsNullOrEmpty(buildNumber) ? buildNumber : null,
                PublishVerificationResults = !string.IsNullOrEmpty(buildNumber)
            };

            //This token is taken from within the broker UI (See settings > Read/write token (CI))
            var options = new PactUriOptions(Config.Broker_Token);

            //Act / Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ProviderState($"{ProviderUri}/provider-states")
                .ServiceProvider("MenuAPI", ProviderUri)
                .HonoursPactWith("GenericMenuConsumer")
                .PactUri($"{CreatePactUri("GenericMenuConsumer", "MenuAPI")}", options)
                .Verify();
        }

        private string CreatePactUri(string consumerName, string providerName)
        {
            return $"{Config.Broker_Url}/pacts/provider/{providerName}/consumer/{consumerName}/latest";
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
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
