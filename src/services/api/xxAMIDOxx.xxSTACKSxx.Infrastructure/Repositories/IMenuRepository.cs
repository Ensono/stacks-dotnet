using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories
{
    //TODO: Move Interface to App and leave just implementations in the Infrastructure

    public interface IMenuRepository
    {
        Task<Menu> GetById(Guid id);
        Task Save(Menu entity);
    }
}
