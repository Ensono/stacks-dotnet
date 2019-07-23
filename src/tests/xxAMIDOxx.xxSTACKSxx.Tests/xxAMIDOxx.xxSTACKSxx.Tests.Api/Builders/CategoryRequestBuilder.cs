using xxAMIDOxx.xxSTACKSxx.Tests.Api.Models;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public class CategoryRequestBuilder : IBuilder<CategoryRequest>
    {
        private CategoryRequest category;

        public CategoryRequestBuilder()
        {
            category = new CategoryRequest();
        }

        public CategoryRequestBuilder WithName(string name)
        {
            category.name = name;
            return this;
        }

        public CategoryRequestBuilder WithDescription(string description)
        {
            category.description = description;
            return this;
        }

        public CategoryRequest Build()
        {
            return category;
        }
    }
}
