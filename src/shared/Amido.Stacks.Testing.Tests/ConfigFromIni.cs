namespace Amido.Stacks.Testing.Tests
{
    public class ConfigFromIni
    {
        public string PROPERTY_FROM_INI { get; set; }
        public SubSectionConfig SubSection { get; set; }

        public class SubSectionConfig
        {
            public int SubProperty { get; set; }
        }
    }
}