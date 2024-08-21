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
/// These are the steps required for testing the Category endpoints
/// </summary>
public class ItemSteps
{
	private readonly MenuSteps menuSteps = new MenuSteps();
	private readonly CategorySteps categorySteps = new CategorySteps();
	private readonly string baseUrl;
	private HttpResponseMessage lastResponse;
	private string existingMenuId;
	private string existingCategoryId;
	private string existingItemId;
	private MenuItemRequest createItemRequest;
	private MenuItemRequest updateItemRequest;
	private const string categoryPath = "/category/";
	private const string itemPath = "/items/";

	public ItemSteps()
	{
		var config = ConfigAccessor.GetApplicationConfiguration();
		baseUrl = config.BaseUrl;
	}

	#region Step Definitions

	#region Given

	public void GivenIHaveSpecfiedAFullItem()
	{
		createItemRequest = new MenuItemBuilder()
			.SetDefaultValues("Yumido Test Item")
			.Build();
	}

	#endregion Given

	#region When

	public async Task WhenISendAnUpdateItemRequest()
	{
		updateItemRequest = new MenuItemBuilder()
			.WithName("Updated item name")
			.WithDescription("Updated item description")
			.WithPrice(4.5)
			.WithAvailablity(true)
			.Build();
		String path =
			$"{MenuSteps.menuPath}{existingMenuId}{categoryPath}{existingCategoryId}{itemPath}{existingItemId}";

		lastResponse = await HttpRequestFactory.Put(baseUrl, path, updateItemRequest);
	}

	public async Task WhenICreateTheItemForAnExistingMenuAndCategory()
	{
		existingMenuId = await menuSteps.GivenAMenuAlreadyExists();
		existingCategoryId = await categorySteps.CreateCategoryForSpecificMenu(existingMenuId);

		lastResponse = await HttpRequestFactory.Post(baseUrl,
			$"{MenuSteps.menuPath}{existingMenuId}{categoryPath}{existingCategoryId}{itemPath}", createItemRequest);
		existingItemId = JsonConvert
			.DeserializeObject<CreateObjectResponse>(await lastResponse.Content.ReadAsStringAsync()).id;
	}

	public async Task WhenIDeleteTheItem()
	{
		lastResponse = await HttpRequestFactory.Delete(baseUrl,
			$"{MenuSteps.menuPath}{existingMenuId}{categoryPath}{existingCategoryId}{itemPath}{existingItemId}");
	}

	#endregion When

	#region Then

	public void ThenTheItemHasBeenCreated()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");
	}

	public async Task ThenTheItemHasBeenDeleted()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

		var getCurrentMenu = await HttpRequestFactory.Get(baseUrl, $"{MenuSteps.menuPath}{existingMenuId}");
		if (getCurrentMenu.StatusCode == HttpStatusCode.OK)
		{
			var getCurrentMenuResponse =
				JsonConvert.DeserializeObject<Menu>(await getCurrentMenu.Content.ReadAsStringAsync());

			getCurrentMenuResponse.categories[0].items.ShouldBeEmpty();
		}
	}

	public async Task ThenTheItemIsUpdatedCorrectly()
	{
		lastResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
			$"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} was not as expected");

		var updatedResponse = await HttpRequestFactory.Get(baseUrl, $"{MenuSteps.menuPath}{existingMenuId}");

		if (updatedResponse.StatusCode == HttpStatusCode.OK)
		{
			var updateItemResponse =
				JsonConvert.DeserializeObject<Menu>(await updatedResponse.Content.ReadAsStringAsync());


			updateItemResponse.categories[0].items[0].name.ShouldBe(updateItemRequest.name,
				$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

			updateItemResponse.categories[0].items[0].description.ShouldBe(updateItemRequest.description,
				$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");

			updateItemResponse.categories[0].items[0].price.ShouldBe(updateItemRequest.price.ToString(),
				$"{lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} did not create the menu as expected");
			updateItemResponse.categories[0].items[0].available.ShouldBeTrue();
		}
		else
		{
			throw new Exception($"Could not retrieve the updated menu using GET /menu/{existingMenuId}");
		}
	}

	#endregion Then

	#endregion Step Definitions
}