using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public interface IMenuItemCommand : ICategoryCommand
    {
        Guid MenuItemId { get; }
    }
}
