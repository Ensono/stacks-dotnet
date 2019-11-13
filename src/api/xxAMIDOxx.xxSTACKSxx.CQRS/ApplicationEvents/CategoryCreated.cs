using System;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Core.Operations;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.ApplicationEvents
{
    public class CategoryCreated : IApplicationEvent
    {
        public CategoryCreated(OperationCode operationCode, Guid correlationId, Guid menuId, Guid categoryId)
        {
            OperationCode = (int)operationCode;
            CorrelationId = correlationId;
            MenuId = menuId;
            CategoryId = categoryId;
        }

        public CategoryCreated(IOperationContext context, Guid menuId, Guid categoryId)
        {
            OperationCode = context.OperationCode;
            CorrelationId = context.CorrelationId;
            MenuId = menuId;
            CategoryId = categoryId;
        }

        public int EventCode => (int)Common.Events.EventCode.CategoryCreated;

        public int OperationCode { get; }

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }
        public Guid CategoryId { get; set; }

    }
}
