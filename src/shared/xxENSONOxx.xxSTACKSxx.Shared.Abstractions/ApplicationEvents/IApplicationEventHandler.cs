namespace xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;

public interface IApplicationEventHandler<in TApplicationEvent> where TApplicationEvent : IApplicationEvent
{
    Task HandleAsync(TApplicationEvent applicationEvent);
}
