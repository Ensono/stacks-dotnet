using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Application.CQRS.Events
{
	public class CosmosDbChangeFeedEvent : IApplicationEvent
	{
		[JsonConstructor]
		public CosmosDbChangeFeedEvent(int operationCode, Guid correlationId, Guid entityId, string eTag)
		{
			OperationCode = operationCode;
			CorrelationId = correlationId;
			EntityId = entityId;
			ETag = eTag;
		}

		public CosmosDbChangeFeedEvent(IOperationContext context, Guid entityId, string eTag)
		{
			OperationCode = context.OperationCode;
			CorrelationId = context.CorrelationId;
			EntityId = entityId;
			ETag = eTag;
		}

		public int EventCode => (int)Enums.EventCode.EntityUpdated;

		public int OperationCode { get; private set; }

		public Guid CorrelationId { get; private set; }

		public Guid EntityId { get; private set; }

		public string ETag { get; private set; }
	}
}
