using xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Models;

namespace xxAMIDOxx.xxSTACKSxx.API.FunctionalTests.Builders;

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
	public CategoryRequestBuilder SetDefaultValues(string name)
	{
		category.name = name;
		category.description = "Category description";
		return this;
	}
}
