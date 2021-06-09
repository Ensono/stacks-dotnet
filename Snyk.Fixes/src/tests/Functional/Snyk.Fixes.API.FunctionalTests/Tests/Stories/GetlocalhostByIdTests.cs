using TestStack.BDDfy;
using Xunit;
using Snyk.Fixes.API.FunctionalTests.Tests.Fixtures;
using Snyk.Fixes.API.FunctionalTests.Tests.Steps;

namespace Snyk.Fixes.API.FunctionalTests.Tests.Functional
{
    //Define the story/feature being tested
    [Story(
        AsA = "user of the Yumido website",
        IWant = "to be able to view specific localhosts",
        SoThat = "I can choose what to eat")]
    public class GetlocalhostByIdTests : IClassFixture<AuthFixture>
    {
        private readonly AuthFixture fixture;
        private readonly localhostSteps steps;

        public GetlocalhostByIdTests(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            steps = new localhostSteps();
        }

        //Add all tests that make up the story to this class.
        [Fact]
        public void Users_Can_View_Existing_localhosts()
        {
            this.Given(s => fixture.GivenAUser())
                .And(s => steps.GivenAlocalhostAlreadyExists())
                .When(s => steps.WhenIGetAlocalhost())
                .Then(s => steps.ThenICanReadThelocalhostReturned())
                .BDDfy();
        }

        [Fact]
        [Trait("Category", "SmokeTest")]
        public void Admins_Can_View_Existing_localhosts()
        {
            this.Given(s => fixture.GivenAnAdmin())
                .And(s => steps.GivenAlocalhostAlreadyExists())
                .When(s => steps.WhenIGetAlocalhost())
                .Then(s => steps.ThenICanReadThelocalhostReturned())
                .BDDfy();
        }
    }
}
