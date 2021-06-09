using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class CreateCategory : IlocalhostCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.CreateCategory;

        public Guid CorrelationId { get; set; }

        public Guid localhostId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CreateCategory(Guid correlationId, Guid localhostId, string name, string description)
        {
            CorrelationId = correlationId;
            localhostId = localhostId;
            Name = name;
            Description = description;
        }
    }
}
