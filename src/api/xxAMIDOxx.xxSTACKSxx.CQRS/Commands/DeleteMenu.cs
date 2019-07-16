using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public partial class DeleteMenu
    {
        public DeleteMenu(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
