using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class CreateCategory : ICategoryCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.CreateCategory;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
