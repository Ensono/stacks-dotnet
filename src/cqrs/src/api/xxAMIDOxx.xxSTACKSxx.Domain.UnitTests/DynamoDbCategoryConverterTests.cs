using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoFixture.Xunit2;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Domain.Converters;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.Domain.UnitTests;

[Trait("TestType", "UnitTests")]
public class DynamoDbCategoryConverterTests
{
    [Theory, AutoData]
    public void CategoryToDynamoDbObject(List<Category> categories)
    {
        // Arrange
        IPropertyConverter converter = new DynamoDbCategoryConverter();

        // Act
        var result = converter.ToEntry(categories);
        var first = result.AsListOfDocument()[0];

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(first);
        Assert.Equal(3, result.AsListOfDocument().Count);

        // Category Fields
        Assert.Equal(categories[0].Id.ToString(), first["Id"]);
        Assert.Equal(categories[0].Name, first["Name"]);
        Assert.Equal(categories[0].Description, first["Description"]);
        Assert.NotNull(first["Items"].AsListOfDocument());
        Assert.Equal(3, first["Items"].AsListOfDocument().Count);

        // MenuItem
        var menuItem = first["Items"].AsListOfDocument()[0];
        Assert.Equal(categories[0].Items[0].Id.ToString(), menuItem["Id"]);
        Assert.Equal(categories[0].Items[0].Description, menuItem["Description"]);
        Assert.Equal(categories[0].Items[0].Name, menuItem["Name"]);
    }

    [Theory, AutoData]
    public void DynamoDbObject(List<Category> categories)
    {
        // Arrange
        IPropertyConverter converter = new DynamoDbCategoryConverter();

        var documents = new List<Document>();
        documents.AddRange(categories.Select(x =>
        {
            var data = new Dictionary<string, DynamoDBEntry>
            {
                { "Id", x.Id },
                { "Description", x.Description },
                { "Name", x.Name }
            };

            return new Document(data);
        }));

        // Act
        var converterResultObj = converter.FromEntry(documents);

        // Assert
        Assert.NotNull(converterResultObj);

        var catResult = converterResultObj as IReadOnlyList<Category>;
        Assert.NotNull(catResult);
        Assert.Equal(3, catResult.Count);

        var first = catResult[0];
        Assert.Equal(categories[0].Id, first.Id);
        Assert.Equal(categories[0].Description, first.Description);
        Assert.Equal(categories[0].Name, first.Name);
    }
}
