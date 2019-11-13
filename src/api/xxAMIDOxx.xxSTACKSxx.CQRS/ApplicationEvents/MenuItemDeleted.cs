using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class MenuItemDeleted : IApplicationEvent
    {
        public MenuItemDeleted(OperationCode operationCode, Guid correlationId, Guid menuId, Guid categoryId, Guid menuItemId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            MenuId = menuId;
            CategoryId = categoryId;
            MenuItemId = menuItemId;
        }

        public MenuItemDeleted(IOperationContext context, Guid menuId, Guid categoryId, Guid menuItemId)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
            MenuId = menuId;
            CategoryId = categoryId;
            MenuItemId = menuItemId;
        }

        public int EventCode => (int)Common.Events.EventCode.MenuItemDeleted;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid MenuItemId { get; set; }
    }
}
