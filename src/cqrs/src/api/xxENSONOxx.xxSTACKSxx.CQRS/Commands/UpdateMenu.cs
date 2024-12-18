using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class UpdateMenu : IMenuCommand
{
    public int OperationCode => (int)Shared.Abstractions.Operations.OperationCode.UpdateMenu;

    public Guid CorrelationId { get; }

    public Guid MenuId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }
}
