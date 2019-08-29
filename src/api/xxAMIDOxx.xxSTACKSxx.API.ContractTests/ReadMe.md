# xxAMIDOxx.xxSTACKSxx.Api.ContractTests

`xxAMIDOxx.xxSTACKSxx.Api.ContractTests` is a sample solution for demostrating Consumer Driven contract tests from the provider point of view.

This project contains **only** the provider side of the contract testing and therefore should work alongsidethe Consumer contract testing solution available here:
https://github.com/amido/stacks-pact-consumer

This project implements contract testing using Pacts - https://pact.io/

## What is Consumer Driven Contract testing?
Consumer Driven Contract (CDC) Testing is a pattern that allows a consumer (i.e: a client) and a provider (i.e. an API provider) to communicate using an agreed contract (a pact).

There is full documentation for how contract testing works available on the [Pact website](https://docs.pact.io/how_pact_works)


## Tools
- PactNet - https://github.com/pact-foundation/pact-net
- xUnit - https://xunit.net/

## How does it work?
The provider side tests at a high level do the following:
1. Create a mocked instance of the provider API
2. Retrieve any consumer pacts from the configured Pact Broker
3. If specified in the contracts, set up the provider state (E.g. Create an existing item so we can test "GET" requests)
4. Verify the Pact against the mocked API
5. Publish results back to the broker

This can be seen within `MenuApiProviderTests.cs`

```c#
        [Theory]
        [InlineData("GenericMenuConsumer")]
        public void EnsureProviderApiHonoursPactWithConsumer(string consumerName)
        {
            var options = new PactUriOptions(Config.Broker_Token);

            //1. Create a mocked instance of the provider API
            using(var ProviderWebHost = WebHost.CreateDefaultBuilder()
                .UseUrls(ProviderUri)
                .UseStartup<TestStartup>()
                .ConfigureServices(DependencyRegistration.ConfigureStaticServices)
                .Build())
            {
                ProviderWebHost.Start();

                //2. Retrieve any consumer pacts from the configured Pact Broker
                VerifyPactFor(consumerName, PactConfig, options);
            }
        }

        private void VerifyPactFor(string consumerName, PactVerifierConfig config, PactUriOptions options)
        {
            IPactVerifier pactVerifier = new PactVerifier(config);

            //3. If specified in the contracts, set up the provider state
            //4. Verify the Pact against the mocked API
            //5. Publish results back to the broker
            pactVerifier
                .ProviderState($"{ProviderUri}/provider-states")
                .ServiceProvider(ProviderName, ProviderUri)
                .HonoursPactWith(consumerName)
                .PactUri($"{CreatePactUri(consumerName, ProviderName)}", options)
                .Verify();
        }
```
