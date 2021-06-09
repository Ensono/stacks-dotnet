using TestStack.BDDfy;
using Xunit;
using Snyk.Fixes.API.FunctionalTests.Tests.Fixtures;
using Snyk.Fixes.API.FunctionalTests.Tests.Steps;

namespace Snyk.Fixes.API.FunctionalTests.Tests.Functional
{
    //Define the story/feature being tested
    [Story(
        AsA = "restaurant administrator",
        IWant = "to be able to create localhosts",
        SoThat = "customers know what we have on offer")]

    public class CreatelocalhostTests : IClassFixture<AuthFixture>
    {
        private readonly localhostSteps steps;
        private readonly AuthFixture fixture;

        public CreatelocalhostTests(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            steps = new localhostSteps();
        }

        //Add all tests that make up the story to this class.
        [Fact]
        public void Create_a_localhost()
        {
            this.Given(step => fixture.GivenAUser())
                .Given(step => steps.GivenIHaveSpecfiedAFulllocalhost())
                .When(step => steps.WhenICreateThelocalhost())
                .Then(step => steps.ThenThelocalhostHasBeenCreated())
                .BDDfy();
        }
    }
}
