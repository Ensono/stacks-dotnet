using System;
using System.Threading.Tasks;
using Snyk.Fixes.Domain;

namespace Snyk.Fixes.Application.Integration
{
    public interface IlocalhostRepository
    {
        Task<localhost> GetByIdAsync(Guid id);
        Task<bool> SaveAsync(localhost entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
