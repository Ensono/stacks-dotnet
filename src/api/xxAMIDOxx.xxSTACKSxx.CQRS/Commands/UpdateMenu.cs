using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class UpdateMenu : IMenuCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.UpdateMenu;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }

}
