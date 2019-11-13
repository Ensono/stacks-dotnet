using AutoFixture.Xunit2;
using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.Domain.UnitTests
{
    [Trait("TestType", "UnitTests")]
    public class MenuTests
    {
        [Theory, AutoData]

        public void Update(Menu menu, string name, string description, bool enabled)
        {
            menu.Update(name, description, enabled);

            Assert.Equal(name, menu.Name);
            Assert.Equal(description, menu.Description);
            Assert.Equal(enabled, menu.Enabled);

            //TODO: When DDD story is complete, check if the events are raised
        }
    }
}
