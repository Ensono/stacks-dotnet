using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Snyk.Fixes.Common.Operations;

namespace Snyk.Fixes.CQRS.ApplicationEvents
{
    public class localhostItemDeleted : IApplicationEvent
    {
        public localhostItemDeleted(OperationCode operationCode, Guid correlationId, Guid localhostId, Guid categoryId, Guid localhostItemId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
            localhostItemId = localhostItemId;
        }

        public localhostItemDeleted(IOperationContext context, Guid localhostId, Guid categoryId, Guid localhostItemId)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
            localhostId = localhostId;
            CategoryId = categoryId;
            localhostItemId = localhostItemId;
        }

        public int EventCode => (int)Common.Events.EventCode.localhostItemDeleted;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid localhostItemId { get; set; }
    }
}
