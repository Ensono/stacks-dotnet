using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class CreateCategory : IMenuCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.CreateCategory;

        public Guid CorrelationId { get; set; }

        public Guid MenuId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
