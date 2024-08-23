using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

/// <summary>
/// Define required parameters for commands executed against a category
/// </summary>
public interface ICategoryCommand : IMenuCommand
{
    Guid CategoryId { get; }
}
