using System.Threading.Tasks;

namespace xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync(ICommand command);
    }
}
