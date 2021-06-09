using System;

namespace Snyk.Fixes.CQRS.Commands
{
    /// <summary>
    /// Define required parameters for commands executed against a category
    /// </summary>
    public interface ICategoryCommand : IlocalhostCommand
    {
        Guid CategoryId { get; }
    }
}
