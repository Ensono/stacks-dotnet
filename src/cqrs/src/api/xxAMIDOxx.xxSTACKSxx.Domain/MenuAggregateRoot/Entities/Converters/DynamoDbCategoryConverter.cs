using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using xxAMIDOxx.xxSTACKSxx.Domain.Entities;

namespace xxAMIDOxx.xxSTACKSxx.Domain.Converters;

// IMPORTANT!!!
// Since Categories is IReadOnlyCollection<T> we need the converter to work with that interface both during serialization
// and deserialization. If Categories was a property using a CONCRETE type like List<T>, then the DynamoSDK would automatically handle
// the collection mapping and we could've made a converter for only a single Category
// DynamoDB .NET Object Persistence Model Types - https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetSDKHighLevel.html#DotNetDynamoDBContext.SupportedTypes
public class DynamoDbCategoryConverter : IPropertyConverter
{
    public DynamoDBEntry ToEntry(object value)
    {
        IEnumerable<Category> categories = value as IReadOnlyCollection<Category>;
        List<Document> entries = new List<Document>();

        if (categories == null) 
            throw new ArgumentOutOfRangeException(nameof(value));

        entries.AddRange(categories.Select(x =>
        {
            var category = JsonConvert.SerializeObject(x);
            return Document.FromJson(category);
        }));

        return entries;
    }

    public object FromEntry(DynamoDBEntry entry)
    {
        var entries = entry as DynamoDBList;
        var documents = entries?.AsListOfDocument();
        if (documents == null)
            throw new ArgumentOutOfRangeException(nameof(entry));

        var categories = documents
            .Select(x => JsonConvert.DeserializeObject<Category>((x.ToJson())))
            .ToList()
            .AsReadOnly();

        return categories;
    }
}
