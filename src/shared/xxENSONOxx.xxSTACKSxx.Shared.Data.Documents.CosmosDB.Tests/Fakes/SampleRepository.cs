using System;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Tests.DataModel;

namespace xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Tests.Fakes
{
    public class SampleRepository(CosmosDbDocumentStorage<SampleEntityExtension> repository)
    {
        //this is a simple and dummy repository for testing real world scenario
        //it shall not be used as reference implementation

        public async Task<SampleEntity> GetByIdAsync(string id, string partition)
        {
            return (await repository.GetByIdAsync(id, partition)).Content;
        }

        public async Task<bool> SaveAsync(SampleEntity entity)
        {
            string eTag = null;
            var dbEntity = entity as SampleEntityExtension;

            if (dbEntity == null)
                dbEntity = SampleEntityExtension.FromSampleEntity(entity);
            else
                eTag = dbEntity.ETag;

            var result = await repository.SaveAsync(entity.Id.ToString(), entity.OwnerId.ToString(), dbEntity, eTag);

            return result.IsSuccessful;
        }

        public async Task<bool> DeleteAsync(Guid id, string partition)
        {
            return (await repository.DeleteAsync(id.ToString(), partition)).IsSuccessful;
        }
    }
}
