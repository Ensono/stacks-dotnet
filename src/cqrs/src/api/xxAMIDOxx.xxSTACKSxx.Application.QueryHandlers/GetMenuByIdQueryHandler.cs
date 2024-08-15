using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Application.CQRS.Queries;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;

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
