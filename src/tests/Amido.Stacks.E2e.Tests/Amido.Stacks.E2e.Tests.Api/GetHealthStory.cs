using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace Amido.Stacks.E2e.Tests.Api
{
    [Story(
        AsA = "Test engineer starting a new project",
        IWant = "A test automation template",
        SoThat = "I can reduce sprint 0 duration")]
    public class GetHealth
    {
        private CrudService _service;
        private HttpResponseMessage _response;

        void GivenIHaveATestEndpoint()
        {
            _service = new CrudService("http://52.151.102.150");
        }

        async Task WhenIMakeAGetRequestToTheEndpoint()
        {
            _response = await _service.Get("/api/menu/health");
        }

        void ThenTheResultShouldIncludeAListOfEndpoints()
        {
            Assert.True(_response.IsSuccessStatusCode);
            Assert.Equal("ok", _response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void HealthShouldBeOK()
        {
            this.BDDfy();
        }
    }
}
