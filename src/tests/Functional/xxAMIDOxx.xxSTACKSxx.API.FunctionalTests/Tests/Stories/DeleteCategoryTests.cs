using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator of a restaurant",
	IWant = "To be able to delete old categories",
	SoThat = "Customers do not see out of date options")]
public class DeleteCategoryTests : IClassFixture<AuthFixture>
{
	private readonly CategorySteps categorySteps;
	private readonly AuthFixture fixture;

	public DeleteCategoryTests(AuthFixture fixture)
	{
		//Get instances of the fixture and steps required for the test
		this.fixture = fixture;
		categorySteps = new CategorySteps();
	}

	//Add all tests that make up the story to this class
	[Fact]
	public void Admins_Can_Delete_Categories()
	{
		this.Given(step => fixture.GivenAUser())
			.And(step => categorySteps.GivenIHaveSpecfiedAFullCategory())
			.When(step => categorySteps.WhenICreateTheCategoryForAnExistingMenu())
			.Then(step => categorySteps.ThenTheCategoryHasBeenCreated())
			.When(step => categorySteps.WhenIDeleteTheCategory())
			.Then(step => categorySteps.ThenTheCategoryHasBeenDeleted())
			.BDDfy();
	}
}
