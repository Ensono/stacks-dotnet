namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;

public class CosmosDbChangeFeedEvent
{
    public string id { get; set; }
    public int OperationCode { get; set; }
    public string CorrelationId { get; set; }
    public string EntityId { get; set; }
    public string ETag { get; set; }
}
