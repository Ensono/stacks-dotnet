using System;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents;
using Serilog;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        static ILogger log = Log.Logger;
        IDocumentStorage<Menu, Guid> documentStorage;

        public MenuRepository(IDocumentStorage<Menu, Guid> documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        public async Task<Menu> GetByIdAsync(Guid id)
        {
            log.Information("Retrieving document {resourceType}({resourceId}) from CosmosDB", typeof(Menu).Name, id);
            var result = await documentStorage.GetByIdAsync(id, id.ToString());

            //TODO: Publish request charge results

            return result.Content;
        }

        public async Task<bool> SaveAsync(Menu entity)
        {
            log.Information("Saving document {resourceType}({resourceId}) to CosmosDB", typeof(Menu).Name, entity.Id);
            //TODO: Handle etag
            //TODO: Publish request charge results

            var result = await documentStorage.SaveAsync(entity.Id, entity.Id.ToString(), entity, null);
            return result.IsSuccessful;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            log.Information("Deleting document {resourceType}({resourceId}) from CosmosDB", typeof(Menu).Name, id);

            //TODO: Publish request charge results

            var result = await documentStorage.DeleteAsync(id, id.ToString());
            return result.IsSuccessful;
        }
    }
}

