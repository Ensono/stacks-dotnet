using System;
using Newtonsoft.Json;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.ApplicationEvents;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Operations;

namespace xxENSONOxx.xxSTACKSxx.Application.CQRS.Events
{
    [method: JsonConstructor]
    public class MenuCreatedEvent(int operationCode, Guid correlationId, Guid menuId) : IApplicationEvent
    {
        public MenuCreatedEvent(IOperationContext context, Guid menuId) : this(context.OperationCode, context.CorrelationId, menuId)
        {
        }

		public int EventCode => (int)Enums.EventCode.MenuCreated;

		public int OperationCode { get; } = operationCode;

        public Guid CorrelationId { get; } = correlationId;

        public Guid MenuId { get; set; } = menuId;
    }
}
