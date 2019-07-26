using System;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.CosmosDB.Tests.DataModel;
using Amido.Stacks.Data.Documents.CosmosDB.Tests.Settings;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Options;
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
            //TODO: Assert other fields
            //Assert.Equal(dbItem.RestaurantId, entity.RestaurantId);
            //Assert.Equal(dbItem.Description, entity.Description);
            //Assert.Equal(dbItem.Enabled, entity.Enabled);
            //Assert.Equal(dbItem.Categories, entity.Categories);
        }

        [Theory, AutoData]
        public async Task UpdateTest(SampleEntity entity)
        {
        }

        [Theory, AutoData]
        public async Task UpdateWithETagTest(SampleEntity entity)
        {
        }

        [Theory, AutoData]
        public async Task DeleteTest(SampleEntity entity)
        {
            await SaveItem(entity);

            var result = await repository.DeleteAsync(entity.Id, GetPartitionKey(entity));
            Assert.True(result);

            var dbItem = await GetItem(entity);
            Assert.Null(dbItem);
        }

        private async Task<SampleEntity> GetItem(SampleEntity entity)
        {
            output.WriteLine($"Retrieving the entity '{entity.Id}' from the repository");
            var dbItem = await repository.GetByIdAsync(entity.Id, GetPartitionKey(entity));
            return dbItem;
        }

        private async Task SaveItem(SampleEntity entity)
        {
            output.WriteLine($"Saving the entity '{entity.Id}' in the repository");
            await repository.SaveAsync(entity, GetPartitionKey(entity), null);
        }

        private string GetPartitionKey(SampleEntity entity)
        {
            return entity.OwnerId.ToString();
        }
    }
}
