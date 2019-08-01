using System;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.CosmosDB.Tests.DataModel;

namespace Amido.Stacks.Data.Documents.CosmosDB.Tests.Fakes
{
    public class SampleRepository
    {
        CosmosDbDocumentStorage<SampleEntity, Guid> repository;

        //this is a simple and dummy repository for testing
        //it shall not be used as reference implementation
        public SampleRepository(CosmosDbDocumentStorage<SampleEntity, Guid> repository)
        {
            this.repository = repository;
        }

        public async Task<SampleEntity> GetByIdAsync(Guid id, string partition)
        {
            return (await repository.GetByIdAsync(id, partition)).Content;
        }

        public async Task<bool> SaveAsync(string partition, SampleEntity entity, string eTag)
        {
            return (await repository.SaveAsync(entity.Id, partition, entity, eTag)).IsSuccessful;
        }

        public async Task<bool> DeleteAsync(Guid id, string partition)
        {
            return (await repository.DeleteAsync(id, partition)).IsSuccessful;
        }
    }
}
