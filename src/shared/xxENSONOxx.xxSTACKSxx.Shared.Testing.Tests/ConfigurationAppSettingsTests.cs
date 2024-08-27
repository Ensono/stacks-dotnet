using xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings;
using Xunit;

namespace xxENSONOxx.xxSTACKSxx.Shared.Testing.Tests
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
