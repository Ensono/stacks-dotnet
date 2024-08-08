using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace Amido.Stacks.Messaging.Events
{
    public class DummyEventAes : IApplicationEvent
    {
		[JsonConstructor]
		public DummyEventAes(int operationCode, Guid correlationId)
		{
			OperationCode = operationCode;
			CorrelationId = correlationId;
		}

		public DummyEventAes(IOperationContext context)
		{
			OperationCode = context.OperationCode;
			CorrelationId = context.CorrelationId;
		}

		public int EventCode => 9871;

		public int OperationCode { get; }

		public Guid CorrelationId { get; }
	}
}
