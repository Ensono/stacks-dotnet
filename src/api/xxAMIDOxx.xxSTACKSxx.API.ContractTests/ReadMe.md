# xxAMIDOxx.xxSTACKSxx.Api.ContractTests

`xxAMIDOxx.xxSTACKSxx.Api.ContractTests` is a sample solution for demostrating Consumer Driven contract tests from the provider point of view.

This project contains **only** the provider side of the contract testing and therefore should work alongsidethe Consumer contract testing solution available here:
https://github.com/amido/stacks-pact-consumer

This project implements contract testing using Pacts - https://pact.io/

## What is Consumer Driven Contract testing?
Consumer Driven Contract (CDC) Testing is a pattern that allows a consumer (i.e: a client) and a provider (i.e. an API provider) to communicate using an agreed contract (a pact).

There is a full documentation for how contract testing works available on the [Pact website](https://docs.pact.io/how_pact_works)


## Tools
- PactNet - https://github.com/pact-foundation/pact-net
- xUnit - https://xunit.net/

## Running Tests

####Note
Please provide the correct values for ```BROKER_URL``` and ```PACT_BEARER_TOKEN``` in ```appsettings.json``` file.

Tests are automatically run as part of the CI pipeline under the task "Run Contract Tests".

To run the tests locally, there are two options. In each scenario, tests will run against the LATEST contract within the PACT broker. 
If the tests need to run against a different version of the contracts, version within the source code will need to be updated. See `private string CreatePactUri(string consumerName, string providerName)` within the class `MenuApiProviderTests.cs`

#### Command line
Navigate to the contract test project folder:

`$ cd Path/To/Repo/src/tests/Functional/xxAMIDOxx.xxSTACKSxx.API.FunctionalTests`

Run DotNet Test:

`$ dotnet test` 

#### Visual Stuido 
Tests can easily be run from within the Test Explorer window in Visual Studio.

Open `Test > Windows > Test Explorer` to open the test explorer, then right-click on a test to run.

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
