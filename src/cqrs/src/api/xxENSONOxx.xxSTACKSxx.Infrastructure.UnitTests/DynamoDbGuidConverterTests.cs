using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoFixture.Xunit2;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Common.Converters;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.UnitTests;

[Trait("TestType", "UnitTests")]
public class DynamoDbGuidConverterTests
{
	[Theory, AutoData]
	public void GuidToDynamoDbObject(Guid id)
	{
		// Arrange
		IPropertyConverter converter = new DynamoDbGuidConverter();

		// Act
		var result = converter.ToEntry(id);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(id.ToString(), result.AsPrimitive().Value.ToString());
	}

	[Theory, AutoData]
	public void DynamoDbObjectToGuid(Guid id)
	{
		// Arrange
		IPropertyConverter converter = new DynamoDbGuidConverter();

		var primitive = new Primitive(id.ToString());

		// Act
		var result = converter.FromEntry(primitive);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<Guid>(result);
		Assert.Equal(id, result);
	}
}
