using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class CreateOrUpdateCategoryBuilder : IBuilder<CreateOrUpdateCategory>
    {
        private CreateOrUpdateCategory model;

        public CreateOrUpdateCategoryBuilder()
        {
            model = new CreateOrUpdateCategory();
        }

        public CreateOrUpdateCategoryBuilder WithName(string name)
        {
            model.name = name;
            return this;
        }

        public CreateOrUpdateCategoryBuilder WithDescription(string description)
        {
            model.description = description;
            return this;
        }

        public CreateOrUpdateCategory Build()
        {
            return model;
        }
    }
}
