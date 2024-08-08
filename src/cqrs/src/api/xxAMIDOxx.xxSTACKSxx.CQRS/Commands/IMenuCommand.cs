using System;
using Amido.Stacks.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

/// <summary>
/// Define required parameters for commands executed against a menu
/// </summary>
public interface IMenuCommand : ICommand
{
    Guid MenuId { get; }
}
