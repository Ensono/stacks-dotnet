using System;

namespace Snyk.Fixes.CQRS.Commands
{
    /// <summary>
    /// Define required parameters for commands executed against a localhost item
    /// </summary>
    public interface IlocalhostItemCommand : ICategoryCommand
    {
        Guid localhostItemId { get; }
    }
}
