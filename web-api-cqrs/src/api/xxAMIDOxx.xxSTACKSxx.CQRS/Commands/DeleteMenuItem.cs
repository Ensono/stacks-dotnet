using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public class DeleteMenuItem : IMenuItemCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.DeleteMenuItem;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid MenuItemId { get; set; }

        public DeleteMenuItem(Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
        {
            CorrelationId = correlationId;
            MenuId = menuId;
            CategoryId = categoryId;
            MenuItemId = menuItemId;
        }
    }
}
