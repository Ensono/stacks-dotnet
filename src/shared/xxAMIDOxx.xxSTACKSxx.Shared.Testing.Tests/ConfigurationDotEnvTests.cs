using Amido.Stacks.Testing.Settings;
using Xunit;

namespace Amido.Stacks.Testing.Tests
{
    public class ConfigurationDotEnvTests
    {
        [Fact]
        [Trait("Category", "env")]
        public void ConfigFor_EnvFile()
        {
            var config = Configuration.For<ConfigFromEnv>();

            //Root
            Assert.NotNull(config);
            Assert.Equal("Value from Env", config.PROPERTY_FROM_ENVVAR);
        }
    }
}