namespace xxENSONOxx.xxSTACKSxx.Shared.Messaging.Handlers.TestDependency
{
    public interface ITestable<T>
    {
        void Complete(T operationContext);
    }
}
