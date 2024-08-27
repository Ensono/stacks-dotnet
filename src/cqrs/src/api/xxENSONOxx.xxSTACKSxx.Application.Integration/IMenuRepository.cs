using System;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Domain;

namespace xxENSONOxx.xxSTACKSxx.Application.Integration;

public interface IMenuRepository
{
    Task<Menu> GetByIdAsync(Guid id);
    Task<bool> SaveAsync(Menu entity);
    Task<bool> DeleteAsync(Guid id);
}
