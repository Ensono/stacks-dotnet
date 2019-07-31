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
        public async Task SearchTest(List<SampleEntity> entities)
        {
            //ARRANGE
            foreach (var entity in entities)
            {
                await SaveItem(entity);
            }


            //TEST 1
            var age = entities.First().Age;
            var result = await repository.Search(filter => filter.Age == age);
            Assert.Contains(result.Content, i => i.Id == entities.First().Id);

            //TEST 2
            var expiry = entities.Last().ExpiryDate;
            var result2 = await repository.Search(filter => filter.ExpiryDate == expiry);
            Assert.Contains(result2.Content, i => i.ExpiryDate == entities.Last().ExpiryDate);

            //TEST 3 - Multilpe results
            var result3 = await repository.Search(filter => filter.Age > 1, pageSize: 1);
            Assert.Contains(result3.Content, i => i.Age > 1);
        }


        [Theory, AutoData]
        public async Task SearchSqlQueryTest(List<SampleEntity> entities)
        {
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
