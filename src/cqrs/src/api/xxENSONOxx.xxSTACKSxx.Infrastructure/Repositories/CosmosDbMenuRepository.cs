#if CosmosDb
using System;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;

public class CosmosDbMenuRepository(Abstractions.IDocumentStorage<Menu> documentStorage) : IMenuRepository
{
    public async Task<Menu> GetByIdAsync(Guid id)
    {
        var result = await documentStorage.GetByIdAsync(id.ToString(), id.ToString());

        //TODO: Publish request charge results

        return result.Content;
    }

    public async Task<bool> SaveAsync(Menu entity)
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
#endif
