using System;
using Amido.Stacks.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    //
    public interface IMenuCommand : ICommand
    {
        Guid MenuId { get; }
    }
}
