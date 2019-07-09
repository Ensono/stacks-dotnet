using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Integration
{
    //TODO: Move Interface to it's own package
    //it will be referenced by:
    // - Commands
    // - Queries
    // - Infrastructure

    public interface IMenuRepository
    {
        Task<Menu> GetByIdAsync(Guid id);
        Task<bool> SaveAsync(Menu entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
