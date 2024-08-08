using Amido.Stacks.Testing.Settings;
using Xunit;

namespace Amido.Stacks.Testing.Tests
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