using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Tests.Api.Builders.Http;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

public class HealthCheckSteps
{
	readonly ConfigModel config;
	readonly string baseUrl;
	HttpResponseMessage lastResponse;

	public HealthCheckSteps()
	{
		config = ConfigAccessor.GetApplicationConfiguration();
		baseUrl = config.BaseUrl;
	}

	public async Task ICheckTheApiHealth()
	{
		lastResponse = await HttpRequestFactory.Get(baseUrl, "health");
	}

	public async Task TheStatusIsHealthy()
	{
		var responseBody = await lastResponse.Content.ReadAsStringAsync();
		responseBody.ShouldBe("Healthy", $"API status is not Healthy. Actual status = {responseBody}");
	}
}
