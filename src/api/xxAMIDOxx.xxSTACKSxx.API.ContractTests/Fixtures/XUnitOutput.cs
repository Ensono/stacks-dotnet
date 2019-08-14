using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace xxAMIDOxx.xxSTACKSxx.API.ContractTests.Fixtures
{
    public class XUnitOutput : IOutput
    {
        private readonly ITestOutputHelper _output;

        public XUnitOutput(ITestOutputHelper output)
        {
            _output = output;
        }

        public void WriteLine(string line)
        {
            _output.WriteLine(line);
        }
    }
}
