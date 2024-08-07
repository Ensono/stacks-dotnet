using System;
using System.Collections;
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
        public string Name { get; private set; }
        public int Age { get; private set; }
        public DateTime DateOfBirth { get; }
        public DateTimeOffset ExpiryDate { get; private set; }
        public string[] EmailAddresses { get; }
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
            EmailAddresses = emailAddresses;
            Siblings = siblings;
            Active = active;
        }

        public void SetNewValues(string newName, int newAge, DateTimeOffset newExpiryDate)
        {
            Name = newName;
            Age = newAge;
            ExpiryDate = newExpiryDate;
        }
    }

    public class Person : IEqualityComparer, IEqualityComparer<Person>
    {
        public string Name { get; }
        public List<Child> Children { get; }

        public Person(string name, List<Child> children)
        {
            Name = name;
            Children = children;
        }

        public bool Equals(Person x, Person y)
        {
            return x.Name == y.Name;

        }
        public new bool Equals(object x, object y)
        {
            return Equals((Person)x, (Person)y);
        }


        public int GetHashCode(Person obj)
        {
            return Name.GetHashCode();
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode((Person)obj);
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


    /// <summary>
    /// PartialEntity used for custom queries
    /// </summary>
    public class PartialEntity
    {

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; }
        public Guid OwnerId { get; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public DateTime DateOfBirth { get; }
        public DateTimeOffset ExpiryDate { get; private set; }
        public string[] EmailAddresses { get; }
        public bool Active { get; }

        public PartialEntity(Guid id, Guid ownerId, string name, int age, DateTime dateOfBirth, DateTimeOffset expiryDate, string[] emailAddresses, bool active)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Age = age;
            DateOfBirth = dateOfBirth;
            ExpiryDate = expiryDate;
            EmailAddresses = emailAddresses;
            Active = active;
        }
    }
}
