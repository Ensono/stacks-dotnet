#if DynamoDb
using System;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.Domain;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Abstractions;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Repositories;

public class DynamoDbMenuRepository(IDynamoDbObjectStorage<Menu> storage) : IMenuRepository
{
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await storage.DeleteAsync(id.ToString());

        return result.IsSuccessful;
    }

    public async Task<Menu> GetByIdAsync(Guid id)
    {
        var result = await storage.GetByIdAsync(id.ToString());

        return result.Content;
    }

    public async Task<bool> SaveAsync(Menu entity)
    {
        var result = await storage.SaveAsync(entity.Id.ToString(), entity);

        return result.IsSuccessful;
    }
}
#endif
