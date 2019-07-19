using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class MenuUpdated : IApplicationEvent
    {
        public MenuUpdated(OperationCode operationCode, Guid correlationId, Guid menuId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            MenuId = menuId;
        }

        public int EventCode => (int)Common.Events.EventCode.MenuUpdated;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }
    }
}
