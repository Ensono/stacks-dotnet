using System.Linq;
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
            var menu = await repository.GetByIdAsync(criteria.Id);

            if (menu == null)
                return null;

            var result = new Menu()
            {
                Id = menu.Id,
                RestaurantId = menu.RestaurantId,
                Name = menu.Name,
                Description = menu.Description,
                Enabled = menu.Enabled,
                Categories = menu.Categories?.Select(c =>
                    new Category()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Items = c.Items?.Select(i =>
                            new MenuItem()
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Description = i.Description,
                                Price = i.Price,
                                Available = i.Available
                            }
                        ).ToList()
                    }).ToList()
            };

            return result;
        }
    }
}
