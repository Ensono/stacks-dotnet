using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        public async Task<Menu> GetById(Guid id) { return await Task.FromResult(new Menu()); }
        public async Task Save(Menu entity) { }
    }
}
