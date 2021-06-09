using System;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.Abstractions;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Infrastructure.Repositories
{
    public class localhostRepository : IlocalhostRepository
    {
        readonly IDocumentStorage<localhost> documentStorage;

        public localhostRepository(IDocumentStorage<localhost> documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        public async Task<localhost> GetByIdAsync(Guid id)
        {
            var result = await documentStorage.GetByIdAsync(id.ToString(), id.ToString());

            //TODO: Publish request charge results

            return result.Content;
        }

        public async Task<bool> SaveAsync(localhost entity)
        {
            //TODO: Handle etag
            //TODO: Publish request charge results

            var result = await documentStorage.SaveAsync(entity.Id.ToString(), entity.Id.ToString(), entity, null);

            return result.IsSuccessful;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            //TODO: Publish request charge results

            var result = await documentStorage.DeleteAsync(id.ToString(), id.ToString());

            return result.IsSuccessful;
        }
    }
}

