using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class DeleteMenu(Guid menuId) : IMenuCommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.DeleteMenu;

    public Guid CorrelationId { get; }

    public Guid MenuId { get; set; } = menuId;
}
