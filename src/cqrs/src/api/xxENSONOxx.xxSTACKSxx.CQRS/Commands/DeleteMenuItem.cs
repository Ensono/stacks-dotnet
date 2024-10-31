using System;

namespace xxENSONOxx.xxSTACKSxx.CQRS.Commands;

public class DeleteMenuItem(Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
    : IMenuItemCommand
{
    public int OperationCode => (int)Shared.Abstractions.Operations.OperationCode.DeleteMenuItem;

    public Guid CorrelationId { get; } = correlationId;

    public Guid MenuId { get; set; } = menuId;

    public Guid CategoryId { get; set; } = categoryId;

    public Guid MenuItemId { get; set; } = menuItemId;
}
