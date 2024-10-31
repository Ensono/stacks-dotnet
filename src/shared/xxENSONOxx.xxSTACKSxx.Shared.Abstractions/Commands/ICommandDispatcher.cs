namespace xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task SendAsync(ICommand command);
}
