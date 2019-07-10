using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class CreateOrUpdateMenuBuilder : IBuilder<CreateOrUpdateMenu>
    {
        private CreateOrUpdateMenu model;

        public CreateOrUpdateMenuBuilder()
        {
            model = new CreateOrUpdateMenu();
        }

        public CreateOrUpdateMenuBuilder SetDefaultValues(string name)
        {
            model.name = name;
            model.description = "This is the default create/update menu description";
            model.enabled = true;
            return this;
        }

        public CreateOrUpdateMenuBuilder WithName(string name)
        {
            model.name = name;
            return this;
        }

        public CreateOrUpdateMenuBuilder WithDescription(string description)
        {
            model.description = description;
            return this;
        }

        public CreateOrUpdateMenuBuilder SetEnabled(bool enabled)
        {
            model.enabled = enabled;
            return this;
        }

        public CreateOrUpdateMenu Build()
        {
            return model;
        }
    }
}
