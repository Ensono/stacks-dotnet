using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snyk.Fixes.Application.Integration;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Infrastructure.Fakes
{
    public class InMemorylocalhostRepository : IlocalhostRepository
    {
        private static readonly Object @lock = new Object();
        private static readonly Dictionary<Guid, localhost> storage = new Dictionary<Guid, localhost>();

        public async Task<localhost> GetByIdAsync(Guid id)
        {
            if (storage.ContainsKey(id))
                return await Task.FromResult(storage[id]);
            else
                return await Task.FromResult((localhost)null);
        }

        public async Task<bool> SaveAsync(localhost entity)
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
