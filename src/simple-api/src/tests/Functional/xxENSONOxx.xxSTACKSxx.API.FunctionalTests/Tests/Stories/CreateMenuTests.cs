using TestStack.BDDfy;
using Xunit;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxENSONOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(
    AsA = "restaurant administrator",
    IWant = "to be able to create menus",
    SoThat = "customers know what we have on offer")]

public class CreateMenuTests(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly MenuSteps steps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class.
    [Fact]
    public void Create_a_menu()
    {
        this.Given(step => fixture.GivenAUser())
            .Given(step => steps.GivenIHaveSpecfiedAFullMenu())
            .When(step => steps.WhenICreateTheMenu())
            .Then(step => steps.ThenTheMenuHasBeenCreated())
            .BDDfy();
    }
}
