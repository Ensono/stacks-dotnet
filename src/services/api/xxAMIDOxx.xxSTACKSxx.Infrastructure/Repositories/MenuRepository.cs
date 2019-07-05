using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Integration;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        static System.Collections.Generic.Dictionary<Guid, Menu> storage = new System.Collections.Generic.Dictionary<Guid, Menu>();

        public async Task<Menu> GetById(Guid id)
        {
            if (storage.ContainsKey(id))
                return await Task.FromResult(storage[id]);
            else
                return await Task.FromResult((Menu)null);
        }

        public async Task Save(Menu entity)
        {
            if (entity == null)
                return;

            if (storage.ContainsKey(entity.Id))
                storage[entity.Id] = entity;
            else
                storage.Add(entity.Id, entity);
        }
    }
}
