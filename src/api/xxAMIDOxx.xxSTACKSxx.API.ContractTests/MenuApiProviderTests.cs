using System.Collections.Generic;
using Amido.Stacks.Tests.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;
using xxAMIDOxx.xxSTACKSxx.API.ContractTests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;

namespace xxAMIDOxx.xxSTACKSxx.API.ContractTests
{
    public class MenuApiProviderTests
    {
        private string ProviderUri { get; }
        private ITestOutputHelper OutputHelper { get; }

        private readonly ConfigModel Config;
        private const string ProviderName = "MenuAPI";
        private readonly PactVerifierConfig PactConfig;

        public MenuApiProviderTests(ITestOutputHelper output)
        {
            OutputHelper = output;

            ProviderUri = "http://localhost:6001";

            //Get application configuration
            //This depends on another stacks project. Just reusing Pact would mean creating your own configuration accessor
            Config = Configuration.For<ConfigModel>();

            //Set up the Pact configuration to be used in tests
            PactConfig = new PactVerifierConfig
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
                ProviderVersion = !string.IsNullOrEmpty(Config.Build_Number) ? Config.Build_Number : null,
                PublishVerificationResults = !string.IsNullOrEmpty(Config.Build_Number)
            };
        }

        [Theory]
        [InlineData("GenericMenuConsumer")]
        public void EnsureProviderApiHonoursPactWithConsumer(string consumerName)
        {
            //This token is taken from within the broker UI (See settings > Read/write token (CI))
            var options = new PactUriOptions(Config.Broker_Token);

            using (var ProviderWebHost = WebHost.CreateDefaultBuilder()
                .UseUrls(ProviderUri)
                .UseStartup<TestStartup>()
                .ConfigureServices(DependencyRegistration.ConfigureStaticDependencies)
                .Build())
            {
                ProviderWebHost.Start();

                VerifyPactFor(consumerName, PactConfig, options);
            }
        }

        private void VerifyPactFor(string consumerName, PactVerifierConfig config, PactUriOptions options)
        {
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ProviderState($"{ProviderUri}/provider-states")
                .ServiceProvider(ProviderName, ProviderUri)
                .HonoursPactWith(consumerName)
                .PactUri($"{CreatePactUri(consumerName, ProviderName)}", options)
                .Verify();
        }

        private string CreatePactUri(string consumerName, string providerName)
        {
            return $"{Config.Broker_Url}/pacts/provider/{providerName}/consumer/{consumerName}/latest";
        }
    }
}
