using TestStack.BDDfy;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Fixtures;
using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Steps;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Tests.Functional;

//Define the story/feature being tested
[Story(
	AsA = "user of the Yumido website",
	IWant = "to be able to view specific menus",
	SoThat = "I can choose what to eat")]
public class GetMenuByIdTests(AuthFixture fixture) : IClassFixture<AuthFixture>
{
    private readonly MenuSteps steps = new();

    //Get instances of the fixture and steps required for the test

    //Add all tests that make up the story to this class.
	[Fact]
	public void Users_Can_View_Existing_Menus()
	{
		this.Given(s => fixture.GivenAUser())
			.And(s => steps.GivenAMenuAlreadyExists())
			.When(s => steps.WhenIGetAMenu())
			.Then(s => steps.ThenICanReadTheMenuReturned())
			.BDDfy();
	}

	[Fact]
	[Trait("Category", "SmokeTest")]
	public void Admins_Can_View_Existing_Menus()
	{
		this.Given(s => fixture.GivenAnAdmin())
			.And(s => steps.GivenAMenuAlreadyExists())
			.When(s => steps.WhenIGetAMenu())
			.Then(s => steps.ThenICanReadTheMenuReturned())
			.BDDfy();
	}
}
