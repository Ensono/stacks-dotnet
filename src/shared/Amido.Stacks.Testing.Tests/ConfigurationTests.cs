using Amido.Stacks.Testing.Settings;
using Xunit;

namespace Amido.Stacks.Testing.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void ConfigRoot()
        {
            var config = Configuration.Root;

            // Assert
            Assert.NotNull(config);
            Assert.Equal("simpleValue", config["SIMPLE_PROPERTY"]);
            Assert.Equal("Value from Env", config["PROPERTY_FROM_ENVVAR"]);
            Assert.Equal("123", config["PROPERTY_FROM_INI"]);
        }

        [Fact]
        public void ConfigFor_SimpleObjectSection()
        {
            var config = Configuration.For<SimpleConfig>("SectionName");

            Assert.NotNull(config);
            Assert.Equal("PropertyValue", config.SectionConfigProperty);
        }

        [Fact]
        public void ConfigFor_NestedObjectSection()
        {
            var config = Configuration.For<NestedConfig>("DeepNesting");

            //Root
            Assert.NotNull(config);
            Assert.Equal("abc", config.Name);
            //Child
            Assert.NotNull(config.Nested);
            Assert.Equal("def", config.Nested.Name);
            //Child-Child
            Assert.NotNull(config.Nested.Nested);
            Assert.Equal("gij", config.Nested.Nested.Name);
        }

        [Fact]
        public void ConfigFor_NestedObject_FilteredSection()
        {
            var config = Configuration.For<NestedConfig>("DeepNesting:Nested:Nested");

            //Root
            Assert.NotNull(config);
            Assert.Equal("gij", config.Name);
        }

        [Fact]
        public void ConfigFor_IniFile()
        {
            var config = Configuration.For<ConfigFromIni>();

            //Root
            Assert.NotNull(config);
            Assert.Equal("123", config.PROPERTY_FROM_INI);
            Assert.NotNull(config.SubSection);
            Assert.Equal(456, config.SubSection.SubProperty);
        }
    }
}