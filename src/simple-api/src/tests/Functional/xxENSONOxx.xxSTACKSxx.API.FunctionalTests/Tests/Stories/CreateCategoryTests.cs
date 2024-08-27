using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Stories;

//Define the story/feature being tested
[Story(
    AsA = "restaurant administrator",
    IWant = "to be able to create menus with categories",
    SoThat = "customers know what we have on offer")]
public class CreateCategoryTests(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly CategorySteps categorySteps = new();


    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class.
    [Fact]
    public void Create_category_for_menu()
    {
        this.Given(step => fixture.GivenAUser())
            .And(step => categorySteps.GivenIHaveSpecfiedAFullCategory())
            .When(step => categorySteps.WhenICreateTheCategoryForAnExistingMenu())
            .Then(step => categorySteps.ThenTheCategoryHasBeenCreated())
            .BDDfy();
    }
}
