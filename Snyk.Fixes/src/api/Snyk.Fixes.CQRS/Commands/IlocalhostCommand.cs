using System;
using Amido.Stacks.Application.CQRS.Commands;

namespace Snyk.Fixes.CQRS.Commands
{
    /// <summary>
    /// Define required parameters for commands executed against a localhost
    /// </summary>
    public interface IlocalhostCommand : ICommand
    {
        Guid localhostId { get; }
    }
}
