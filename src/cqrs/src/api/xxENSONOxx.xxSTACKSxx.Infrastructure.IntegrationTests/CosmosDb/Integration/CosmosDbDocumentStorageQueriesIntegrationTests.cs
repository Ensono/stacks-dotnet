#if (CosmosDb)
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;
using xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.DataModel;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.Integration;

/// <summary>
/// The purpose of this integration test is to validate the implementation
/// of CosmosDbDocumentStorage against the data store at development\integration
/// It is intended to test:
/// - Communitation with CosmosDB
/// - Encoding and Parsing of data
/// - ...
/// </summary>
[Trait("TestType", "IntegrationTests")]
[Collection(IntegrationCollection.Name)]
public class CosmosDbDocumentStorageQueriesIntegrationTests
{
    private readonly ITestOutputHelper output;
    CosmosDbDocumentStorage<SampleEntity> repository;

    public CosmosDbDocumentStorageQueriesIntegrationTests(ITestOutputHelper output, IntegrationFixture integrationFixture)
    {
        this.output = output;
        var settings = Configuration.For<CosmosDbConfiguration>("CosmosDB");
        var loggerFactory = Substitute.For<ILoggerFactory>();
        Fixture fixture = new Fixture();
        fixture.Register<ILogger<CosmosDbDocumentStorage<SampleEntity>>>(() => new Logger<CosmosDbDocumentStorage<SampleEntity>>(loggerFactory));
        fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
        fixture.Register<IOptions<CosmosDbConfiguration>>(() => settings.AsOption());

        repository = fixture.Create<CosmosDbDocumentStorage<SampleEntity>>();
    }

    [Theory, AutoData]
    public async Task SearchSingleFieldTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var age = entities.First().Age;

        //ACT
        var result = await repository.Search(filter => filter.Age == age);

        //ASSERT
        Assert.Contains(result.Content, i => i.Id == entities.First().Id);
    }

    [Theory, AutoData]
    public async Task SearchSingleFieldWithCustomReturnTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var age = entities.First().Age;

        //ACT
        var result = await repository.Search<PartialEntity>(filter => filter.Age == age);

        //ASSERT
        Assert.Contains(result.Content, i => i.Id == entities.First().Id);
    }

    [Theory, AutoData]
    public async Task SearchMultipleFieldsTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);

        var age = entities.Last().Age;
        var expiry = entities.Last().ExpiryDate;

        //ACT
        var result = await repository.Search(filter => filter.Age == age && filter.ExpiryDate == expiry);

        //ASSERT
        Assert.Contains(result.Content, i => i.Id == entities.Last().Id);
    }

    [Theory, AutoData]
    public async Task SearchSingleFieldWithLimitTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var id = entities.First().Id;

        //ACT
        var result = await repository.Search(filter => filter.Id == id, pageSize: 10);

        //ASSERT
        Assert.Single(result.Content);
        Assert.Contains(result.Content, i => i.Id == id);
    }

    [Theory, AutoData]
    public async Task SearchSingleFieldOrderedAscWithLimitTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var id = entities.First().Id;

        //ACT
        var result = await repository.Search<SampleEntity, int>(
            filter => filter.Age > 1,
            order => order.Age,
            pageSize: 10);

        //ASSERT
        var items = result.Content.ToList();
        for (int i = 1; i < items.Count(); i++)
        {
            //Ensure current age is bigger or equal previous
            Assert.True(items[i].Age >= items[i - 1].Age);
        }
    }


    [Theory, AutoData]
    public async Task SearchSingleFieldOrderedDescWithLimitTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var id = entities.First().Id;

        //ACT
        var result = await repository.Search<SampleEntity, int>(
            filter => filter.Age > 1,
            order => order.Age,
            isAscendingOrder: false,
            pageSize: 10);

        //ASSERT
        var items = result.Content.ToList();
        for (int i = 1; i < items.Count(); i++)
        {
            //Ensure current age is bigger or equal previous
            Assert.True(items[i].Age <= items[i - 1].Age);
        }
    }

    [Theory, AutoData]
    public async Task SearchSingleFieldWithPartitionTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var owner = entities.First().OwnerId;

        //ACT
        var result = await repository.Search(
            filter => true,
            partitionKey: owner.ToString(),
            pageSize: 10);

        //ASSERT
        Assert.All(result.Content, r =>
            Assert.Equal(r.OwnerId, owner)
        );
    }

    [Theory, AutoData]
    public async Task SearchSqlQueryTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var OwnerId = entities.First().OwnerId;

        //ACT

        var result = await repository.RunSQLQueryAsync<PartialEntity>($"SELECT * FROM c WHERE c.OwnerId = '{OwnerId}'");

        //At least one
        Assert.Contains(result.Content, r =>
            r.OwnerId == OwnerId
        );

        //All should have same OwnerId
        Assert.All(result.Content, r =>
            Assert.Equal(r.OwnerId, OwnerId)
        );
    }

    [Theory, AutoData]
    public async Task SearchSqlQueryWithParameterTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var OwnerId = entities.First().OwnerId;

        var param = new Dictionary<string, object>()
        {
            { "OwnerId", OwnerId.ToString() }
        };

        //ACT
        var result = await repository.RunSQLQueryAsync<PartialEntity>($"SELECT * FROM c WHERE c.OwnerId = @OwnerId", param);

        //At least one
        Assert.Contains(result.Content, r =>
            r.OwnerId == OwnerId
        );

        //All should have same OwnerId
        Assert.All(result.Content, r =>
            Assert.Equal(r.OwnerId, OwnerId)
        );
    }

    [Theory, AutoData]
    public async Task SearchSqlQueryWithContinuationParameterTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var Age = 1;

        var param = new Dictionary<string, object>()
        {
            { "Age", Age }
        };

        //ACT
        var result = await repository.RunSQLQueryAsync<SampleEntity>($"SELECT * FROM c WHERE c.Age > @Age", param);

        //At least one
        Assert.Contains(result.Content, r =>
            r.Age > 1
        );

        //All should have age > 1
        Assert.All(result.Content, r =>
            Assert.True(r.Age > 1)
        );

        var resultContinuation = await repository.RunSQLQueryAsync<SampleEntity>($"SELECT * FROM c WHERE c.Age > @Age", param, null, null, result.Attributes["ContinuationToken"]);

        //Does not contain results returned on previous query
        Assert.DoesNotContain(resultContinuation.Content, c =>
            result.Content.Any(r => r.Id == c.Id)
        );
    }

    [Theory, AutoData]
    public async Task SearchSqlQueryWithContinuationParameterAndLimitResultsTest(List<SampleEntity> entities)
    {
        //ARRANGE
        await SaveItems(entities);
        var Age = 1;

        var param = new Dictionary<string, object>()
        {
            { "Age", Age }
        };

        //ACT
        var result = await repository.RunSQLQueryAsync<SampleEntity>($"SELECT * FROM c WHERE c.Age > @Age", param, null, 2, null);

        //At least one
        Assert.Contains(result.Content, r =>
            r.Age > 1
        );

        //All should have age > 1
        Assert.All(result.Content, r =>
            Assert.True(r.Age > 1)
        );

        //GEt continuation results
        var resultContinuation = await repository.RunSQLQueryAsync<SampleEntity>($"SELECT * FROM c WHERE c.Age > @Age", param, null, 2, result.Attributes["ContinuationToken"]);

        //Does not contain results returned on previous query
        Assert.DoesNotContain(resultContinuation.Content, c =>
            result.Content.Any(r => r.Id == c.Id)
        );
    }

    //SUPPORT METHODS

    private async Task<OperationResult> SaveItem(SampleEntity entity, string eTag = null)
    {
        output.WriteLine($"Saving the entity '{entity.Id}' in the repository");
        return await repository.SaveAsync(entity.Id.ToString(), GetPartitionKey(entity), entity, eTag);
    }

    private async Task SaveItems(List<SampleEntity> entities)
    {
        foreach (var entity in entities)
            await SaveItem(entity);
    }

    private string GetPartitionKey(SampleEntity entity)
    {
        return entity.OwnerId.ToString();
    }
}
#endif
