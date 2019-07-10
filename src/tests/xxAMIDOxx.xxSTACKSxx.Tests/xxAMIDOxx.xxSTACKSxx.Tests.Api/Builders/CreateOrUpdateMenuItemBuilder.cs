using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class CreateOrUpdateMenuItemBuilder : IBuilder<CreateOrUpdateMenuItem>
    {
        private CreateOrUpdateMenuItem model;

        public CreateOrUpdateMenuItemBuilder()
        {
            model = new CreateOrUpdateMenuItem();
        }

        public CreateOrUpdateMenuItemBuilder WithName(string name)
        {
            model.name = name;
            return this;
        }

        public CreateOrUpdateMenuItemBuilder WithDescription(string description)
        {
            model.description = description;
            return this;
        }

        public CreateOrUpdateMenuItemBuilder WithPrice(double price)
        {
            model.price = price;
            return this;
        }

        public CreateOrUpdateMenuItemBuilder SetAvailablity(bool available)
        {
            model.available = available;
            return this;
        }

        public CreateOrUpdateMenuItem Build()
        {
            return model;
        }
    }
}
