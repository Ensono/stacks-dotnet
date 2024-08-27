using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxENSONOxx.xxSTACKSxx.Application.QueryHandlers;

public class GetMenuByIdQueryHandler(IMenuRepository repository) : IQueryHandler<GetMenuById, Menu>
{
    public async Task<Menu> ExecuteAsync(GetMenuById criteria)
    {
        var menu = await repository.GetByIdAsync(criteria.Id);

        if (menu == null)
            return null;

        var result = Menu.FromDomain(menu);

        return result;
    }
}
