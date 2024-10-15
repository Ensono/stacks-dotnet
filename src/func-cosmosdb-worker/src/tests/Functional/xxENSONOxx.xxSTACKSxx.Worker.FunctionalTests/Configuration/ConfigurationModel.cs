namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;

public class ConfigurationModel
{
    public string CosmosDbConnectionString { get; set; }
    public string CosmosDbDatabaseName { get; set; }
    public string CosmosDbContainerName { get; set; }

    public string ServiceBusClientConnectionString { get; set; }
    public string ServiceBusAdminConnectionString { get; set; }
    public string TopicExample { get; set; }
    public string SubsExample { get; set; }
}
