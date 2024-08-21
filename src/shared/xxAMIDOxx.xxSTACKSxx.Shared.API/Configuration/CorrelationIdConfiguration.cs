namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Configuration
{
    public class CorrelationIdConfiguration
    {
        private const string DefaultHeaderName = "x-correlation-id";

        public string HeaderName { get; set; } = DefaultHeaderName;

        public bool IncludeInResponse { get; set; } = true;
    }
}
