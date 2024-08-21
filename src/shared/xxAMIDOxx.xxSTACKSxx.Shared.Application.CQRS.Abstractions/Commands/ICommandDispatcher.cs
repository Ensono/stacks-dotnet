using System.Threading.Tasks;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync(ICommand command);
    }
}
