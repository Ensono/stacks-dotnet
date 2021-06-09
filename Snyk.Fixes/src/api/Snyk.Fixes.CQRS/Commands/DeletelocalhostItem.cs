using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class DeletelocalhostItem : IlocalhostItemCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.DeletelocalhostItem;

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid localhostItemId { get; set; }

        public DeletelocalhostItem(Guid correlationId, Guid localhostId, Guid categoryId, Guid localhostItemId)
        {
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
            localhostItemId = localhostItemId;
        }
    }
}
