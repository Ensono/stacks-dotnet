using System;
using System.Threading.Tasks;
using Amido.Stacks.DynamoDB.Abstractions;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

public class DynamoDbMenuRepository : IMenuRepository
{
    private readonly IDynamoDbObjectStorage<Menu> objectStorage;

    public DynamoDbMenuRepository(IDynamoDbObjectStorage<Menu> storage)
    {
        this.objectStorage = storage;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await objectStorage.DeleteAsync(id.ToString());

        return result.IsSuccessful;
    }

    public async Task<Menu> GetByIdAsync(Guid id)
    {
        var result = await objectStorage.GetByIdAsync(id.ToString());

        return result.Content;
    }

    public async Task<bool> SaveAsync(Menu entity)
    {
        var result = await objectStorage.SaveAsync(entity.Id.ToString(), entity);

        return result.IsSuccessful;
    }
}
