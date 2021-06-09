using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Snyk.Fixes.Common.Operations;

namespace Snyk.Fixes.CQRS.ApplicationEvents
{
    public class localhostCreated : IApplicationEvent
    {
        public localhostCreated(OperationCode operationCode, Guid correlationId, Guid localhostId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            localhostId = localhostId;
        }

        public localhostCreated(IOperationContext context, Guid localhostId)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
            localhostId = localhostId;
        }


        public int EventCode => (int)Common.Events.EventCode.localhostCreated;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

    }
}
