namespace Amido.Stacks.Configuration
{
    public class Secret
    {
        public string Source { get; set; } = "Environment";
        public string Identifier { get; set; }
        public bool Optional { get; set; }

        public static explicit operator Secret(string name) => new Secret() { Identifier = name };
    }
}
