using System;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.Commands
{
    public class UpdateCategory : ICategoryCommand
    {
        public int OperationCode => (int)Common.Operations.OperationCode.UpdateCategory;

        public Guid CorrelationId { get; }

        public Guid MenuId { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UpdateCategory(Guid correlationId, Guid menuId, Guid categoryId, string name, string description)
        {
            CorrelationId = correlationId;
            MenuId = menuId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
        }
    }
}
