using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        static System.Collections.Generic.Dictionary<Guid, Menu> storage = new System.Collections.Generic.Dictionary<Guid, Menu>();

        public async Task<Menu> GetByIdAsync(Guid id)
        {
            if (storage.ContainsKey(id))
                return await Task.FromResult(storage[id]);
            else
                return await Task.FromResult((Menu)null);
        }

        public async Task<bool> SaveAsync(Menu entity)
        {
            if (entity == null)
                return await Task.FromResult(false);

            if (storage.ContainsKey(entity.Id))
                storage[entity.Id] = entity;
            else
                storage.Add(entity.Id, entity);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!storage.ContainsKey(id))
                return false;

            return await Task.FromResult(storage.Remove(id));
        }

    }
}
