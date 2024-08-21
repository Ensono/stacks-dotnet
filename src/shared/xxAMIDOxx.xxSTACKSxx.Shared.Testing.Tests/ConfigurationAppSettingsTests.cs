using xxAMIDOxx.xxSTACKSxx.Shared.Testing.Settings;
using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Testing.Tests
{
    public class ConfigurationAppSettingsTests
    {
        [Fact]
        [Trait("Category", "appsettings")]
        public void ConfigFor_SimpleObjectSection_DevelopmentFile()
        {
            var config = Configuration.For<SimpleConfig>("SectionName");

            Assert.NotNull(config);
            Assert.Equal("SecondaryValue", config.SecondaryProperty);
        }
    }
}