using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Tests.DataModel
{
    /*
     * This is an extensibility example that can be used with CosmosDB Storage or other implementations
     * Instead leaking Infrastructure details (JSon Serialization, ETag, etc..) to the AggreagateRoot
     * that will be stored in the database, we can exten the aggregate to add the fields needed
     * by infrastructure and not leak these details to the domain
    */
    [Table("SampleEntity")]
    public class SampleEntityExtension(
        Guid id,
        Guid ownerId,
        string name,
        int age,
        DateTime dateOfBirth,
        DateTimeOffset expiryDate,
        string[] emailAddresses,
        List<Person> siblings,
        bool active)
        : SampleEntity(id, ownerId, name, age, dateOfBirth, expiryDate, emailAddresses, siblings, active)
    {
        public string DisplayName => $"{Name} ({Age} yo)";

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }

        public static SampleEntityExtension FromSampleEntity(SampleEntity entity)
        {
            // This is a dummy converter used only for testing
            // This implementation will open room for failures when new fields are added and 
            // people forget to update this parser
            // In a real implementation you should consider a more robust approach like automapper

            return new SampleEntityExtension(
                entity.Id,
                entity.OwnerId,
                entity.Name,
                entity.Age,
                entity.DateOfBirth,
                entity.ExpiryDate,
                entity.EmailAddresses,
                entity.Siblings,
                entity.Active
            );
        }
    }
}
