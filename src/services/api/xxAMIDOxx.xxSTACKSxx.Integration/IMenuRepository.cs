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
        Task<Menu> GetById(Guid id);
        Task Save(Menu entity);
    }
}
