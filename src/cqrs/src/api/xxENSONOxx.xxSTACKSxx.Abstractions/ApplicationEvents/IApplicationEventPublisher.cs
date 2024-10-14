namespace xxENSONOxx.xxSTACKSxx.Abstractions.ApplicationEvents;

public interface IApplicationEventPublisher
{
    Task PublishAsync(IApplicationEvent applicationEvent);
}
