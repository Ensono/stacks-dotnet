using TestStack.BDDfy;
using Xunit;
using Snyk.Fixes.API.FunctionalTests.Tests.Fixtures;
using Snyk.Fixes.API.FunctionalTests.Tests.Steps;

namespace Snyk.Fixes.API.FunctionalTests.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator for a restaurant",
        IWant = "To be able to update existing localhosts",
        SoThat = "The localhosts are always showing our latest offerings"
        )]
    public class UpdatelocalhostById : IClassFixture<AuthFixture>
    {
        private readonly AuthFixture fixture;
        private readonly localhostSteps steps;

        public UpdatelocalhostById(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            steps = new localhostSteps();
        }

        //Add all tests that make up the story to this class
        [Fact]
        public void Admins_Can_Update_Existing_localhosts()
        {
            this.Given(s => fixture.GivenAnAdmin())
                .And(s => steps.GivenAlocalhostAlreadyExists())
                .When(s => steps.WhenISendAnUpdatelocalhostRequest())
                .Then(s => steps.ThenThelocalhostIsUpdatedCorrectly())
                .BDDfy();
        }
    }
}
