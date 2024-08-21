using System;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands;

public class CreateMenu(Guid correlationId, Guid tenantId, string name, string description, bool enabled)
    : ICommand
{
    public int OperationCode => (int)Common.Operations.OperationCode.CreateMenu;

    public Guid CorrelationId { get; } = correlationId;

    public Guid TenantId { get; set; } = tenantId;

    public string Name { get; set; } = name;

    public string Description { get; set; } = description;

    public bool Enabled { get; set; } = enabled;
}
