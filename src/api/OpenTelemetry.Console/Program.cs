using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Console
{
    internal class Program
    {
        private static readonly ActivitySource MyActivitySource = new ActivitySource("MyCompany.MyProduct.MyLibrary");

        static void Main(string[] args)
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddSource("MyCompany.MyProduct.MyLibrary")
                .AddConsoleExporter()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyApplicationName"))
                .Build();

            using (var activity = MyActivitySource.StartActivity("SayHello"))
            {
                activity?.SetTag("foo", 1);
                activity?.SetTag("bar", "Hello, World!");
                activity?.SetTag("baz", new int[] { 1, 2, 3 });
            }
        }
    }
}
