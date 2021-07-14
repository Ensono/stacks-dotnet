using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public class UpdateMenuItem : IMenuItemCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.UpdateMenuItem;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid MenuItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public UpdateMenuItem(Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId, string name, string description, double price, bool available)
        {
            CorrelationId = correlationId;
            MenuId = menuId;
            CategoryId = categoryId;
            MenuItemId = menuItemId;
            Name = name;
            Description = description;
            Price = price;
            Available = available;
        }
    }
}
