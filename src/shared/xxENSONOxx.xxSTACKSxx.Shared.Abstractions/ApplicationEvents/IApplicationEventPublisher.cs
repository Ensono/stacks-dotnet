namespace xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

public interface IApplicationEventPublisher
{
    Task PublishAsync(IApplicationEvent applicationEvent);
}
