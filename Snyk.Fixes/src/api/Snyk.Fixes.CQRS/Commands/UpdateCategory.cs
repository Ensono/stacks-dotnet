using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class UpdateCategory : ICategoryCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.UpdateCategory;

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UpdateCategory(Guid correlationId, Guid localhostId, Guid categoryId, string name, string description)
        {
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
        }
    }
}
