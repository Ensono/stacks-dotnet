using System;
using Amido.Stacks.Application.CQRS.Commands;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public class CreateMenu : ICommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.CreateMenu;

        public Guid CorrelationId { get; }

        public Guid TenantId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public CreateMenu(Guid correlationId, Guid tenantId, string name, string description, bool enabled)
        {
            CorrelationId = correlationId;
            TenantId = tenantId;
            Name = name;
            Description = description;
            Enabled = enabled;
        }
    }
}
