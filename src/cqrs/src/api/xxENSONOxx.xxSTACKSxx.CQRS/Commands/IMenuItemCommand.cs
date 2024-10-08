using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

/// <summary>
/// Define required parameters for commands executed against a menu item
/// </summary>
public interface IMenuItemCommand : ICategoryCommand
{
    Guid MenuItemId { get; }
}
