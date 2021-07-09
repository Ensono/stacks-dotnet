using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public class DeleteMenu : IMenuCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.DeleteMenu;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public DeleteMenu(Guid menuId)
        {
            this.MenuId = menuId;
        }
    }
}
