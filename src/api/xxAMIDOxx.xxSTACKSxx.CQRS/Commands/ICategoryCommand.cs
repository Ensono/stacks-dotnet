using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public interface ICategoryCommand : IMenuCommand
    {
        Guid CategoryId { get; }
    }
}
