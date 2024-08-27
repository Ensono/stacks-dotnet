using System;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

/// <summary>
/// Define required parameters for commands executed against a menu
/// </summary>
public interface IMenuCommand : ICommand
{
    Guid MenuId { get; }
}
