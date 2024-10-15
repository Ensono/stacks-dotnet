namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models;

public class CosmosChangeFeedEvent
{
    public string Id { get; set; }
    public int OperationCode { get; set; }
    public string CorrelationId { get; set; }
    public string EntityId { get; set; }
    public string ETag { get; set; }
}
