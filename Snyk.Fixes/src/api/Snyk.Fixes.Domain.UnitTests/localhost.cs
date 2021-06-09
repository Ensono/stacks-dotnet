using AutoFixture.Xunit2;
using Xunit;

namespace Snyk.Fixes.Domain.UnitTests
{
    [Trait("TestType", "UnitTests")]
    public class localhostTests
    {
        [Theory, AutoData]

        public void Update(localhost localhost, string name, string description, bool enabled)
        {
            localhost.Update(name, description, enabled);

            Assert.Equal(name, localhost.Name);
            Assert.Equal(description, localhost.Description);
            Assert.Equal(enabled, localhost.Enabled);

            //TODO: When DDD story is complete, check if the events are raised
        }
    }
}
