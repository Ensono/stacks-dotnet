using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class UpdateCategory : ICategoryCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.UpdateCategory;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
