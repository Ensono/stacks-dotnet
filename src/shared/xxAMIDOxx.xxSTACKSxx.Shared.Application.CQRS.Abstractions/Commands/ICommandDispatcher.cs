using System.Threading.Tasks;

namespace Amido.Stacks.Application.CQRS.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync(ICommand command);
    }
}
