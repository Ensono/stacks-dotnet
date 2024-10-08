using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(AsA = "Administrator for a restaurant",
    IWant = "To be able to update existing categories",
    SoThat = "The menus are always showing our latest offerings"
)]
public class UpdateCategoryById(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly CategorySteps categorySteps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class
    [Fact]
    public void Admins_Can_Update_Existing_Categories()
    {
        this.Given(s => fixture.GivenAnAdmin())
            .And(step => categorySteps.GivenIHaveSpecfiedAFullCategory())
            .When(step => categorySteps.WhenICreateTheCategoryForAnExistingMenu())
            .Then(step => categorySteps.ThenTheCategoryHasBeenCreated())
            .When(s => categorySteps.WhenISendAnUpdateCategoryRequest())
            .Then(s => categorySteps.ThenTheCategoryIsUpdatedCorrectly())
            .BDDfy();
    }
}
