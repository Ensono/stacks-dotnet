using TestStack.BDDfy;
using Xunit;
using Snyk.Fixes.API.FunctionalTests.Tests.Fixtures;
using Snyk.Fixes.API.FunctionalTests.Tests.Steps;

namespace Snyk.Fixes.API.FunctionalTests.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator of a restaurant",
        IWant = "To be able to delete old localhosts",
        SoThat = "Customers do not see out of date options")]
    public class DeletelocalhostTests : IClassFixture<AuthFixture>
    {
        private readonly localhostSteps steps;
        private readonly AuthFixture fixture;

        public DeletelocalhostTests(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            steps = new localhostSteps();
        }

        //Add all tests that make up the story to this class
        [Fact]
        public void Admins_Can_Delete_localhosts()
        {
            this.Given(step => fixture.GivenAUser())
                .And(step => steps.GivenAlocalhostAlreadyExists())
                .When(step => steps.WhenIDeleteAlocalhost())
                .Then(step => steps.ThenThelocalhostHasBeenDeleted())
                .BDDfy();
        }
    }
}
