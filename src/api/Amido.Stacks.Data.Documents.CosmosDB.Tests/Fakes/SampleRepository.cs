using System;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.CosmosDB.Tests.DataModel;

namespace Amido.Stacks.Data.Documents.CosmosDB.Tests.Fakes
{
    public class SampleRepository
    {
        CosmosDbDocumentRepository<SampleEntity, Guid> repository;

        public SampleRepository(CosmosDbDocumentRepository<SampleEntity, Guid> repository)
        {
            this.repository = repository;
        }

        public async Task<SampleEntity> GetByIdAsync(Guid id, string partition)
        {
            return await repository.GetByIdAsync(id, partition);
        }

        public async Task<bool> SaveAsync(SampleEntity entity, string partition, string eTag)
        {
            return await repository.SaveAsync(entity, partition, eTag);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
