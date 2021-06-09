using TestStack.BDDfy;
using Xunit;
using Snyk.Fixes.API.FunctionalTests.Tests.Fixtures;
using Snyk.Fixes.API.FunctionalTests.Tests.Steps;

namespace Snyk.Fixes.API.FunctionalTests.Tests.Functional
{
    //Define the story/feature being tested
    [Story(AsA = "Administrator for a restaurant",
        IWant = "To be able to update existing categories",
        SoThat = "The localhosts are always showing our latest offerings"
        )]
    public class UpdateCategoryById : IClassFixture<AuthFixture>
    {
        private readonly AuthFixture fixture;
        private readonly CategorySteps categorySteps;

        public UpdateCategoryById(AuthFixture fixture)
        {
            //Get instances of the fixture and steps required for the test
            this.fixture = fixture;
            categorySteps = new CategorySteps();
        }

        //Add all tests that make up the story to this class
        [Fact]
        public void Admins_Can_Update_Existing_Categories()
        {
            this.Given(s => fixture.GivenAnAdmin())
                .And(step => categorySteps.GivenIHaveSpecfiedAFullCategory())
                .When(step => categorySteps.WhenICreateTheCategoryForAnExistinglocalhost())
                .Then(step => categorySteps.ThenTheCategoryHasBeenCreated())
                .When(s => categorySteps.WhenISendAnUpdateCategoryRequest())
                .Then(s => categorySteps.ThenTheCategoryIsUpdatedCorrectly())
                .BDDfy();
        }
    }
}
