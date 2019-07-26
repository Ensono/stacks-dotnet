using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amido.Stacks.Data.Documents.CosmosDB.Tests.DataModel
{
    /// <summary>
    /// This is a sample entity to test cosmos db package
    /// This represents an aggregate root in a ordinary project
    /// It should implement at least one property of each type to ensure parsing consistency
    /// </summary>
    public class SampleEntity
    {

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; }
        public Guid OwnerId { get; }
        public string Name { get; }
        public int Age { get; }
        public DateTime DateOfBirth { get; }
        public DateTimeOffset ExpiryDate { get; }
        public string[] emailAddresses { get; }
        public List<Person> Siblings { get; }
        public bool Active { get; }

        public SampleEntity(Guid id, Guid ownerId, string name, int age, DateTime dateOfBirth, DateTimeOffset expiryDate, string[] emailAddresses, List<Person> siblings, bool active)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Age = age;
            DateOfBirth = dateOfBirth;
            ExpiryDate = expiryDate;
            this.emailAddresses = emailAddresses;
            Siblings = siblings;
            Active = active;
        }

    }

    public class Person
    {
        public string Name { get; }
        public List<Child> Children { get; }

        public Person(string name, List<Child> children)
        {
            Name = name;
            Children = children;
        }
    }

    public class Child
    {
        public string Name { get; }
        public int Age { get; }

        public Child(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
