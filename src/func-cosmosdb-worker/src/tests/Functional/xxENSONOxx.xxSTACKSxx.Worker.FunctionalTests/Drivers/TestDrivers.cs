using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;
using xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.ServiceBus;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Drivers;

public class TestDrivers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestDrivers"/> class.
    /// </summary>
    public TestDrivers()
    {
        ServiceBusDriver = new ServiceBusDriver(ConfigAccessor.GetApplicationConfiguration().ServiceBusConnection);
    }

    /// <summary>
    /// Gets an instance of the <see cref="ServiceBusDriver"/>.
    /// </summary>
    public ServiceBusDriver ServiceBusDriver { get; }
}
