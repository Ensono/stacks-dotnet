using System;
using System.Threading.Tasks;
using Amido.Stacks.Data.Documents.Abstractions;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

public class CosmosDbMenuRepository : IMenuRepository
{
    readonly IDocumentStorage<Menu> documentStorage;

    public CosmosDbMenuRepository(IDocumentStorage<Menu> documentStorage)
    {
        this.documentStorage = documentStorage;
    }

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
