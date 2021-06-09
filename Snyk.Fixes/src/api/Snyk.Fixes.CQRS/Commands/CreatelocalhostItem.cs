using System;

namespace Snyk.Fixes.CQRS.Commands
{
    public class CreatelocalhostItem : ICategoryCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.CreatelocalhostItem;

        public Guid CorrelationId { get; set; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public CreatelocalhostItem(Guid correlationId, Guid localhostId, Guid categoryId, string name, string description, double price, bool available)
        {
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Price = price;
            Available = available;
        }
    }
}
