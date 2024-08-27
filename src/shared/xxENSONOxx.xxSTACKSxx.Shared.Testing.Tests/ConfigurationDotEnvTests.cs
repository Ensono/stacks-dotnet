using xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings;
using Xunit;

namespace xxENSONOxx.xxSTACKSxx.Shared.Testing.Tests
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
