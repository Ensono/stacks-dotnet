using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class UpdatelocalhostItem : IlocalhostItemCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.UpdatelocalhostItem;

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid localhostItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public UpdatelocalhostItem(Guid correlationId, Guid localhostId, Guid categoryId, Guid localhostItemId, string name, string description, double price, bool available)
        {
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
            localhostItemId = localhostItemId;
            Name = name;
            Description = description;
            Price = price;
            Available = available;
        }
    }
}
