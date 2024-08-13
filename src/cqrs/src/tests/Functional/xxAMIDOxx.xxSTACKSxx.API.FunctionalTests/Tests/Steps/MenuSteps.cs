using Newtonsoft.Json;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Configuration;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;
using xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders.Http;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

/// <summary>
/// These are the steps required for testing the Menu endpoints
/// </summary>
public class MenuSteps
{
	private MenuRequest createMenuRequest;
	private MenuRequest updateMenuRequest;
	private HttpResponseMessage lastResponse;
	public string existingMenuId;
	private readonly string baseUrl;
	public const string menuPath = "v1/menu/";

	public MenuSteps()
	{
		var config = ConfigAccessor.GetApplicationConfiguration();
		baseUrl = config.BaseUrl;
	}

	#region Step Definitions

	#region Given

	public async Task<string> GivenAMenuAlreadyExists()
	{
		createMenuRequest = new MenuRequestBuilder()
			.SetDefaultValues("Yumido Test Menu")
			.Build();

		try
		{
			lastResponse = await HttpRequestFactory.Post(baseUrl, menuPath, createMenuRequest);

			if (lastResponse.StatusCode == HttpStatusCode.Created)
			{
				existingMenuId = JsonConvert
					.DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
			}
			else
			{
				throw new Exception();
			}
		}
		catch
		{
			throw new Exception(
				$"Menu could not be created. API response: {await lastResponse.Content.ReadAsStringAsync()}");
		}

		return existingMenuId;
	}

	public void GivenIHaveSpecfiedAFullMenu()
	{
		createMenuRequest = new MenuRequestBuilder()
			.SetDefaultValues("Yumido Test Menu")
			.Build();
	}

	#endregion Given

	#region When

	public async Task WhenISendAnUpdateMenuRequest()
	{
		updateMenuRequest = new MenuRequestBuilder()
			.WithName("Updated Menu Name")
			.WithDescription("Updated Description")
			.SetEnabled(true)
			.Build();

		lastResponse = await HttpRequestFactory.Put(baseUrl, $"{menuPath}{existingMenuId}", updateMenuRequest);
	}

	public async Task WhenICreateTheMenu()
	{
		lastResponse = await HttpRequestFactory.Post(baseUrl, menuPath, createMenuRequest);
	}

	public async Task WhenIDeleteAMenu()
	{
		lastResponse = await HttpRequestFactory.Delete(baseUrl, $"{menuPath}{existingMenuId}");
	}

	public async Task WhenIGetAMenu()
	{
		lastResponse = await HttpRequestFactory.Get(baseUrl, $"{menuPath}{existingMenuId}");
	}

	#endregion When

	#region Then

	public void ThenTheMenuHasBeenCreated()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
	}

	public void ThenTheMenuHasBeenDeleted()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
	}

	public async Task ThenICanReadTheMenuReturned()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.OK,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

		var responseMenu = JsonConvert.DeserializeObject<Menu>(await lastResponse.Content.ReadAsStringAsync());

		//compare the initial request sent to the API against the actual response
		responseMenu.name.ShouldBe(createMenuRequest.name,
			$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

		responseMenu.description.ShouldBe(createMenuRequest.description,
			$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

		responseMenu.enabled.ShouldBe(createMenuRequest.enabled,
			$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");
	}

	public async Task ThenTheMenuIsUpdatedCorrectly()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

		var updatedResponse = await HttpRequestFactory.Get(baseUrl, $"{menuPath}{existingMenuId}");

		if (updatedResponse.StatusCode == HttpStatusCode.OK)
		{
			var updateMenuResponse =
				JsonConvert.DeserializeObject<Menu>(await updatedResponse.Content.ReadAsStringAsync());

			updateMenuResponse.name.ShouldBe(updateMenuRequest.name,
				$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

			updateMenuResponse.description.ShouldBe(updateMenuRequest.description,
				$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

			updateMenuResponse.enabled.ShouldBe(updateMenuRequest.enabled,
				$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");
		}
		else
		{
			//throw exception rather than use assertions if the GET request fails as GET is not the subject of the test
			//Assertions should only be used on the subject of the test
			throw new Exception($"Could not retrieve the updated menu using GET /menu/{existingMenuId}");
		}
	}

	#endregion Then

	#endregion Step Definitions
}