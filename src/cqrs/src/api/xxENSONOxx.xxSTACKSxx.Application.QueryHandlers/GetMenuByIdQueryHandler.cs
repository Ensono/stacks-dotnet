using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Application.Integration;
using xxENSONOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
using xxENSONOxx.xxSTACKSxx.Shared.Abstractions.Queries;

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
