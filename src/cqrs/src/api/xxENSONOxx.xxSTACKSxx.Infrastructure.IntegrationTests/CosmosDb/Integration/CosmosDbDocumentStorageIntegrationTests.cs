using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;
using xxENSONOxx.xxSTACKSxx.Common.Exceptions.CosmosDb;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.DataModel;
using xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.Fakes;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents;
using xxENSONOxx.xxSTACKSxx.Shared.Testing.Extensions;
using xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings;
using Config = xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.Integration;

/// <summary>
/// The purpose of this integration test is to validate the implementation
/// of CosmosDbDocumentStorage against the data store at development\integration
/// It is intended to test:
/// - Communication with CosmosDB
/// - Encoding and Parsing of data
/// - ...
/// </summary>
[Trait("TestType", "IntegrationTests")]
[Collection(IntegrationCollection.Name)]
public class CosmosDbDocumentStorageIntegrationTests
{
    private readonly ITestOutputHelper output;
    private readonly CosmosDbDocumentStorage<SampleEntity> repository;
    private Fixture fixture = new();

    public CosmosDbDocumentStorageIntegrationTests(ITestOutputHelper output)
    {
        this.output = output;
        var settings = Config.For<CosmosDbConfiguration>("CosmosDB");
        var loggerFactory = Substitute.For<ILoggerFactory>();

        fixture.Register<ILogger<CosmosDbDocumentStorage<SampleEntity>>>(() => new Logger<CosmosDbDocumentStorage<SampleEntity>>(loggerFactory));
        fixture.Register<ILogger<CosmosDbDocumentStorage<SampleEntityExtension>>>(() => new Logger<CosmosDbDocumentStorage<SampleEntityExtension>>(loggerFactory)); //for named collection
        fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
        fixture.Register(() => settings.AsOption());

        repository = fixture.Create<CosmosDbDocumentStorage<SampleEntity>>();
    }

    [Theory, AutoData]
    public async Task SaveAndGetTest(SampleEntity entity)
    {
        await SaveItem(entity);

        var dbItem = await GetItem(entity);

        Assert.NotNull(dbItem);
        Assert.Equal(dbItem.Id, entity.Id);
        Assert.Equal(dbItem.Name, entity.Name);
        Assert.Equal(dbItem.OwnerId, entity.OwnerId);
        Assert.Equal(dbItem.Age, entity.Age);
        Assert.Equal(dbItem.DateOfBirth, entity.DateOfBirth);
        Assert.Equal(dbItem.ExpiryDate, entity.ExpiryDate);
        Assert.Equal(dbItem.EmailAddresses, entity.EmailAddresses);
        Assert.Equal(dbItem.Active, entity.Active);

        Assert.Equal(
            JsonConvert.SerializeObject(dbItem.Siblings),
            JsonConvert.SerializeObject(entity.Siblings)
        );
    }

    [Theory, AutoData]
    public async Task UpdateWithoutETagTest(SampleEntity entity)
    {
        await CreateAndUpdateEntity(entity, null);
    }

    [Theory, AutoData]
    public async Task UpdateWithValidETagTest(SampleEntity entity)
    {
        var result = await CreateAndUpdateEntity(entity, null);

        entity.SetNewValues("name", 33, DateTimeOffset.UtcNow);

        var updateResult = await SaveItem(entity, result.Attributes["ETag"]);

        Assert.True(updateResult.IsSuccessful);

        var dbItem = await GetItem(entity);
        Assert.NotNull(dbItem);
        Assert.Equal(entity.Name, dbItem.Name);
        Assert.Equal(entity.Age, dbItem.Age);
        Assert.Equal(entity.ExpiryDate, dbItem.ExpiryDate);
        //Check non changed attributes
        Assert.Equal(dbItem.Id, entity.Id);
        Assert.Equal(dbItem.OwnerId, entity.OwnerId);
        Assert.Equal(dbItem.DateOfBirth, entity.DateOfBirth);
        Assert.Equal(dbItem.EmailAddresses, entity.EmailAddresses);
        Assert.Equal(dbItem.Active, entity.Active);

    }

    [Theory, AutoData]
    public async Task UpdateWithInvalidETagTest(SampleEntity entity, string eTag)
    {
        await Assert.ThrowsAsync<DocumentHasChangedException>(async () => await CreateAndUpdateEntity(entity, eTag));
    }

    [Theory, AutoData]
    public async Task DeleteTest(SampleEntity entity)
    {
        await SaveItem(entity);

        var result = await repository.DeleteAsync(entity.Id.ToString(), GetPartitionKey(entity));
        Assert.True(result.IsSuccessful);

        var dbItem = await GetItem(entity);
        Assert.Null(dbItem);
    }

    //SUPPORT METHODS

    private async Task<SampleEntity> GetItem(SampleEntity entity)
    {
        output.WriteLine($"Retrieving the entity '{entity.Id}' from the repository");
        var dbItem = await repository.GetByIdAsync(entity.Id.ToString(), GetPartitionKey(entity));
        return dbItem.Content;
    }

    private async Task<OperationResult> SaveItem(SampleEntity entity, string eTag = null)
    {
        output.WriteLine($"Saving the entity '{entity.Id}' in the repository");
        return await repository.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), entity, eTag);
    }

    private async Task<OperationResult> CreateAndUpdateEntity(SampleEntity entity, string eTag)
    {
        //Arrange
        await SaveItem(entity);
        var dbItem = await GetItem(entity);
        Assert.NotNull(dbItem);

        fixture = new Fixture();
        var newName = fixture.Create<string>();
        var newAge = fixture.Create<int>();
        var newExpiryDate = fixture.Create<DateTimeOffset>();

        //ACT
        dbItem.SetNewValues(newName, newAge, newExpiryDate);

        var result = await SaveItem(dbItem, eTag);

        //ASSERT
        var dbItemModified = await GetItem(entity);

        Assert.Equal(newName, dbItemModified.Name);
        Assert.Equal(newAge, dbItemModified.Age);
        Assert.Equal(newExpiryDate, dbItemModified.ExpiryDate);

        return result;
    }

    private string GetPartitionKey(SampleEntity entity)
    {
        return entity.OwnerId.ToString();
    }


    [Theory, AutoData]
    public async Task ChildEntityWithNamedCollectionTest(SampleEntityExtension entity)
    {
        //Arrange
        var repo2 = fixture.Create<CosmosDbDocumentStorage<SampleEntityExtension>>();

        //Act
        await repo2.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), entity, null);

        //Assert
        var result = await repo2.GetByIdAsync(entity.Id.ToString(), GetPartitionKey(entity));
        Assert.NotNull(result.Content);

        var dbItem = result.Content;

        Assert.Equal(entity.Id, dbItem.Id);
        Assert.Equal(entity.Name, dbItem.Name);
        Assert.Equal(entity.Age, dbItem.Age);
        Assert.Equal(entity.ExpiryDate, dbItem.ExpiryDate);
        Assert.Equal(entity.OwnerId, dbItem.OwnerId);
        Assert.Equal(entity.DateOfBirth, dbItem.DateOfBirth);
        Assert.Equal(entity.EmailAddresses, dbItem.EmailAddresses);
        Assert.Equal(entity.Active, dbItem.Active);

        //When returned from base type, should also be returned
        var resultBase = await repository.GetByIdAsync(entity.Id.ToString(), GetPartitionKey(entity));
        Assert.NotNull(resultBase.Content);
        var dbItem2 = result.Content;

        Assert.Equal(entity.Id, dbItem2.Id);
        Assert.Equal(entity.Name, dbItem2.Name);
        Assert.Equal(entity.Age, dbItem2.Age);
        Assert.Equal(entity.ExpiryDate, dbItem2.ExpiryDate);
        Assert.Equal(entity.OwnerId, dbItem2.OwnerId);
        Assert.Equal(entity.DateOfBirth, dbItem2.DateOfBirth);
        Assert.Equal(entity.EmailAddresses, dbItem2.EmailAddresses);
        Assert.Equal(entity.Active, dbItem2.Active);
    }

    [Theory, AutoData]
    public async Task ChildEntityETAgUpdates(SampleEntityExtension entity)
    {
        //Arrange
        var repo2 = fixture.Create<CosmosDbDocumentStorage<SampleEntityExtension>>();

        //Act
        await repo2.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), entity, null);

        var result1 = await repo2.GetByIdAsync(entity.Id.ToString(), GetPartitionKey(entity));

        await repo2.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), result1.Content, result1.Content.ETag);

        await Assert.ThrowsAsync<DocumentHasChangedException>(async () => await repo2.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), result1.Content, result1.Content.ETag));
        //Assert
        var result2 = await repo2.GetByIdAsync(entity.Id.ToString(), GetPartitionKey(entity));

        Assert.NotEqual(result1.Content.ETag, result2.Content.ETag);

        await repo2.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), result2.Content, result2.Content.ETag);
    }

    [Theory, AutoData]
    public async Task RepositoryWithBaseEntityToChildStorage(SampleEntity entity)
    {
        //INSERT SCENARIO ---------------------------------------------------

        //ARRANGE
        var repository = fixture.Create<SampleRepository>();

        //ACT for INSERT
        await repository.SaveAsync(entity);

        var dbItemCreated = await repository.GetByIdAsync(entity.Id.ToString(), GetPartitionKey(entity));
        //ASSERT
        Assert.Equal(entity.Name, dbItemCreated.Name);


        //UPDATE SCENARIO ----------------------------------------------------

        //ARRANGE
        var newName = "DIEGO";
        dbItemCreated.With(i => i.Name, newName);

        //ACT
        await repository.SaveAsync(dbItemCreated);
        var dbItemUpdated = await repository.GetByIdAsync(dbItemCreated.Id.ToString(), dbItemCreated.OwnerId.ToString());

        //ASSERT
        Assert.Equal(newName, dbItemUpdated.Name);
    }
}
