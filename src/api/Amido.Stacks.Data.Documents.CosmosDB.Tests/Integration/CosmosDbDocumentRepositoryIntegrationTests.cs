using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.CosmosDB.Exceptions;
using Amido.Stacks.Data.Documents.CosmosDB.Tests.DataModel;
using Amido.Stacks.Data.Documents.CosmosDB.Tests.Settings;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Amido.Stacks.Data.Documents.CosmosDB.Tests.Integration
{
    /// <summary>
    /// The purpose of this integration test is to validate the implementation
    /// of CosmosDBRepository againt the data store at development\integration
    /// It is intended to test:
    /// - Communitation with CosmosDB
    /// - Encoding and Parsing of data
    /// - ...
    /// </summary>
    [Trait("TestType", "IntegrationTests")]
    public class CosmosDbDocumentRepositoryIntegrationTests
    {
        private readonly ITestOutputHelper output;
        CosmosDbDocumentRepository<SampleEntity, Guid> repository;

        public CosmosDbDocumentRepositoryIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;
            Fixture fixture = new Fixture();
            fixture.Register<IOptions<CosmosDbConfiguration>>(() =>
                Configuration.For<CosmosDbConfiguration>("CosmosDB").AsOption<CosmosDbConfiguration>()
            );

            //var test = fixture.Create<IOptions<CosmosDbConfiguration>>();
            repository = fixture.Create<CosmosDbDocumentRepository<SampleEntity, Guid>>();
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

            var result = await repository.DeleteAsync(entity.Id, GetPartitionKey(entity));
            Assert.True(result.IsSuccessful);

            var dbItem = await GetItem(entity);
            Assert.Null(dbItem);
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
        public async Task SearchMultiplefieldsTest(List<SampleEntity> entities)
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

            var param = new Dictionary<string, string>()
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

        //SUPPORT METHODS

        private async Task<SampleEntity> GetItem(SampleEntity entity)
        {
            output.WriteLine($"Retrieving the entity '{entity.Id}' from the repository");
            var dbItem = await repository.GetByIdAsync(entity.Id, GetPartitionKey(entity));
            return dbItem.Content;
        }

        private async Task<OperationResult> SaveItem(SampleEntity entity, string eTag = null)
        {
            output.WriteLine($"Saving the entity '{entity.Id}' in the repository");
            return await repository.SaveAsync(entity.Id, GetPartitionKey(entity), entity, eTag);
        }

        private async Task SaveItems(List<SampleEntity> entities)
        {
            foreach (var entity in entities)
                await SaveItem(entity);
        }

        private async Task<OperationResult> CreateAndUpdateEntity(SampleEntity entity, string eTag)
        {
            //Arrange
            await SaveItem(entity);
            var dbItem = await GetItem(entity);
            Assert.NotNull(dbItem);

            Fixture fixture = new Fixture();
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
    }
}
