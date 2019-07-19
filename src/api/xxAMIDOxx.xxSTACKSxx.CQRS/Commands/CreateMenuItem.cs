using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class CreateMenuItem : IMenuItemCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.CreateMenuItem;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid MenuItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double? Price { get; set; }

        public bool? Available { get; set; }
    }
}
