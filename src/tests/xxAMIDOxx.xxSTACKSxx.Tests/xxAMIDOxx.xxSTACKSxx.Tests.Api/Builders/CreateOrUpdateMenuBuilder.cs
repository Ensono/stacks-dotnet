using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class CreateOrUpdateMenuBuilder : IBuilder<CreateOrUpdateMenu>
    {
        private CreateOrUpdateMenu createOrUpdateMenu;

        public CreateOrUpdateMenuBuilder()
        {
            createOrUpdateMenu = new CreateOrUpdateMenu();
        }

        public CreateOrUpdateMenuBuilder SetDefaultValues()
        {
            createOrUpdateMenu.name = "Default test menu name";
            createOrUpdateMenu.description = "This is the default create/update menu description";
            createOrUpdateMenu.enabled = true;
            return this;
        }

        public CreateOrUpdateMenuBuilder WithName(string name)
        {
            createOrUpdateMenu.name = name;
            return this;
        }

        public CreateOrUpdateMenuBuilder WithDescription(string description)
        {
            createOrUpdateMenu.description = description;
            return this;
        }

        public CreateOrUpdateMenuBuilder SetEnabled(bool enabled)
        {
            createOrUpdateMenu.enabled = enabled;
            return this;
        }

        public CreateOrUpdateMenu Build()
        {
            return createOrUpdateMenu;
        }
    }
}
