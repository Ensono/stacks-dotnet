namespace xxENSONOxx.xxSTACKSxx.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task SendAsync(ICommand command);
}
