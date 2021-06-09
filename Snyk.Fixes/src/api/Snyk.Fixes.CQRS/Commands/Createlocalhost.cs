using System;
using Amido.Stacks.Application.CQRS.Commands;

namespace Snyk.Fixes.CQRS.Commands
{
    public class Createlocalhost : ICommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.Createlocalhost;

        public Guid CorrelationId { get; }

        public Guid TenantId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public Createlocalhost(Guid correlationId, Guid tenantId, string name, string description, bool enabled)
        {
            CorrelationId = correlationId;
            TenantId = tenantId;
            Name = name;
            Description = description;
            Enabled = enabled;
        }
    }
}
