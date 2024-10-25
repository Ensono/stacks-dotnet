using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;

public class TestDrivers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestDrivers"/> class.
    /// </summary>
    public TestDrivers()
    {
        CosmosDbDriver = new CosmosDbDriver(ConfigurationAccessor.GetApplicationConfiguration().CosmosDbConnectionString);
        ServiceBusDriver = new ServiceBusDriver(
            ConfigurationAccessor.GetApplicationConfiguration().ServiceBusClientConnectionString,
            ConfigurationAccessor.GetApplicationConfiguration().ServiceBusAdminConnectionString
        );
    }

    /// <summary>
    /// Gets an instance of the <see cref="CosmosDbDriver"/>.
    /// </summary>
    public CosmosDbDriver CosmosDbDriver { get; }

    /// <summary>
    /// Gets an instance of the <see cref="ServiceBusDriver"/>.
    /// </summary>
    public ServiceBusDriver ServiceBusDriver { get; }
}
