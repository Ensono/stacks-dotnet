using System.Threading.Tasks;
using Amido.Stacks.Application.CQRS.Queries;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers
{
    public class GetMenuByIdQueryHandler : IQueryHandler<GetMenuByIdQueryCriteria, Menu>
    {
        private IMenuRepository repository;

        public GetMenuByIdQueryHandler(IMenuRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Menu> ExecuteAsync(GetMenuByIdQueryCriteria criteria)
        {
            //TODO: get Menu from DB, map to result
            var menu = await repository.GetByIdAsync(criteria.Id);

            if (menu == null)
                return null;

            //TODO: Convert domain to models
            var result = new Menu()
            {
                Id = menu.Id,
                RestaurantId = menu.RestaurantId,
                Name = menu.Name,
                Description = menu.Description,
                Enabled = menu.Enabled
            };

            return result;
        }
    }
}
