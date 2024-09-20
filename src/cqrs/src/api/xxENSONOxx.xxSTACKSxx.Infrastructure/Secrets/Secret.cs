namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

public class Secret
{
    public string Source { get; set; } = "Environment";
    public string Identifier { get; set; }
    public bool Optional { get; set; }

    public static explicit operator Secret(string name) => new() { Identifier = name };
}
