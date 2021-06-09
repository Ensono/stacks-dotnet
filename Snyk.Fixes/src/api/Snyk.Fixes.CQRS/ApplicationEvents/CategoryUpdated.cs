using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using Snyk.Fixes.Common.Operations;

namespace Snyk.Fixes.CQRS.ApplicationEvents
{
    public class CategoryUpdated : IApplicationEvent
    {
        public CategoryUpdated(OperationCode operationCode, Guid correlationId, Guid localhostId, Guid categoryId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            localhostId = localhostId;
            CategoryId = categoryId;
        }

        public CategoryUpdated(IOperationContext context, Guid localhostId, Guid categoryId)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
            localhostId = localhostId;
            CategoryId = categoryId;
        }

        public int EventCode => (int)Common.Events.EventCode.CategoryUpdated;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid localhostId { get; set; }

        public Guid CategoryId { get; set; }

    }
}
