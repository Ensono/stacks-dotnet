using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events
{
	public class MenuCreatedEvent : IApplicationEvent
	{
		[JsonConstructor]
		public MenuCreatedEvent(int operationCode, Guid correlationId, Guid menuId)
		{
			OperationCode = operationCode;
			CorrelationId = correlationId;
			MenuId = menuId;
		}

		public MenuCreatedEvent(IOperationContext context, Guid menuId)
		{
			OperationCode = context.OperationCode;
			CorrelationId = context.CorrelationId;
			MenuId = menuId;
		}

		public int EventCode => (int)Enums.EventCode.MenuCreated;

		public int OperationCode { get; }

		public Guid CorrelationId { get; }

		public Guid MenuId { get; set; }
	}
}
