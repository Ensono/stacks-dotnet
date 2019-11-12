using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class MenuDeleted : IApplicationEvent
    {
        public MenuDeleted(OperationCode operationCode, Guid correlationId, Guid menuId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            MenuId = menuId;
        }

        public MenuDeleted(IOperationContext context, Guid menuId)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
            MenuId = menuId;
        }

        public int EventCode => (int)Common.Events.EventCode.MenuDeleted;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }
    }
}
