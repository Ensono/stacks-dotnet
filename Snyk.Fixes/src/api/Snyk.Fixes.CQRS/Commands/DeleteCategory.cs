using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class DeleteCategory : ICategoryCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.DeleteCategory;

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

        public DeleteCategory(Guid correlationId, Guid localhostId, Guid categoryId)
        {
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
        }
    }
}
