using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models.CloudEvents;

public class CloudEvent
{
    public const string MediaType = "application/cloudevents";

    [JsonProperty("datacontenttype")]
    public string DataContentType { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("dataschema")]
    public Uri DataSchema { get; set; }

    [JsonProperty("source")]
    public Uri Source { get; set; }

    [JsonProperty("specversion")]
    public string SpecVersion { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("time")]
    public DateTime? Time { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}
