using xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Drivers.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Drivers;

public class TestDrivers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestDrivers"/> class.
    /// </summary>
    public TestDrivers()
    {
        ServiceBusDriver = new ServiceBusDriver(
            ConfigurationAccessor.GetApplicationConfiguration().ServiceBusClientConnectionString,
            ConfigurationAccessor.GetApplicationConfiguration().ServiceBusAdminConnectionString
        );
    }

    /// <summary>
    /// Gets an instance of the <see cref="ServiceBusDriver"/>.
    /// </summary>
    public ServiceBusDriver ServiceBusDriver { get; }
}
