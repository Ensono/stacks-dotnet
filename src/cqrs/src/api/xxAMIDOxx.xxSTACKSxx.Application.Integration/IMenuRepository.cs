using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Application.Integration;

public interface IMenuRepository
{
    Task<Menu> GetByIdAsync(Guid id);
    Task<bool> SaveAsync(Menu entity);
    Task<bool> DeleteAsync(Guid id);
}
