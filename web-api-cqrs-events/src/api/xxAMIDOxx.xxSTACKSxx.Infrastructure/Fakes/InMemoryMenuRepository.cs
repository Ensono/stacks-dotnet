using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes
{
    public class InMemoryMenuRepository : IMenuRepository
    {
        private static readonly Object @lock = new Object();
        private static readonly Dictionary<Guid, Menu> storage = new Dictionary<Guid, Menu>();

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

            lock (@lock)
            {
                if (storage.ContainsKey(entity.Id))
                    storage[entity.Id] = entity;
                else
                    storage.Add(entity.Id, entity);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool result;
            lock (@lock)
            {
                if (!storage.ContainsKey(id))
                    return false;

                result = storage.Remove(id);
            }

            return await Task.FromResult(result);
        }

    }
}
