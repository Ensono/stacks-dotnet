#if CosmosDb
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.DataModel;

/// <summary>
/// This is a sample entity to test cosmos db package
/// This represents an aggregate root in a ordinary project
/// It should implement at least one property of each type to ensure parsing consistency
/// </summary>
public class SampleEntity(
    Guid id,
    Guid ownerId,
    string name,
    int age,
    DateTime dateOfBirth,
    DateTimeOffset expiryDate,
    string[] emailAddresses,
    List<Person> siblings,
    bool active)
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; } = id;

    public Guid OwnerId { get; } = ownerId;
    public string Name { get; private set; } = name;
    public int Age { get; private set; } = age;
    public DateTime DateOfBirth { get; } = dateOfBirth;
    public DateTimeOffset ExpiryDate { get; private set; } = expiryDate;
    public string[] EmailAddresses { get; } = emailAddresses;
    public List<Person> Siblings { get; } = siblings;
    public bool Active { get; } = active;

    public void SetNewValues(string newName, int newAge, DateTimeOffset newExpiryDate)
    {
        Name = newName;
        Age = newAge;
        ExpiryDate = newExpiryDate;
    }
}

public class Person(string name, List<Child> children) : IEqualityComparer, IEqualityComparer<Person>
{
    public string Name { get; } = name;
    public List<Child> Children { get; } = children;

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

public class Child(string name, int age)
{
    public string Name { get; } = name;
    public int Age { get; } = age;
}


/// <summary>
/// PartialEntity used for custom queries
/// </summary>
public class PartialEntity(
    Guid id,
    Guid ownerId,
    string name,
    int age,
    DateTime dateOfBirth,
    DateTimeOffset expiryDate,
    string[] emailAddresses,
    bool active)
{

    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; } = id;

    public Guid OwnerId { get; } = ownerId;
    public string Name { get; private set; } = name;
    public int Age { get; private set; } = age;
    public DateTime DateOfBirth { get; } = dateOfBirth;
    public DateTimeOffset ExpiryDate { get; private set; } = expiryDate;
    public string[] EmailAddresses { get; } = emailAddresses;
    public bool Active { get; } = active;
}
#endif
