namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Azure.ServiceBus.Tests.Handlers.TestDependency
{
    public interface ITestable<T>
    {
        void Complete(T operationContext);
    }
}
