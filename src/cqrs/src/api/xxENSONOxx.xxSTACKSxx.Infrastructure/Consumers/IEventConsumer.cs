namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Consumers;

public interface IEventConsumer
{
    Task ProcessAsync();
}
