using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace xxAMIDOxx.xxSTACKSxx.Consumer.PactTests.Fixtures
{
    public class MenuApiMock : IDisposable
    {
        public IPactBuilder PactBuilder { get; }

        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort => 9222;

        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public MenuApiMock()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\..\pacts",
                LogDir = @".\pact_logs"
            };

            PactBuilder = new PactBuilder(pactConfig)
                .ServiceConsumer("GenericMenuConsumer")
                .HasPactWith("MenuAPI");

            MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}

