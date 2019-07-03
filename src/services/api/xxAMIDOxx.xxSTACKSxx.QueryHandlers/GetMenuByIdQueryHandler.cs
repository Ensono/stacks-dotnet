using Amido.Stacks.Application.CQRS;
using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;
using xxAMIDOxx.xxSTACKSxx.Models;
using xxAMIDOxx.xxSTACKSxx.Models.Queries;

namespace xxAMIDOxx.xxSTACKSxx.QueryHandlers
{
    public class GetMenuByIdQueryHandler : IQueryHandler<GetMenuByIdQuery, Menu>
    {
        private IMenuRepository repository;

        public GetMenuByIdQueryHandler(IMenuRepository repository)
        {
            this.repository = repository;
        }

        public Task<Menu> Execute(GetMenuByIdQuery criteria)
        {
            //TODO: get Menu from DB, map to result
            //var menu = repository.GetById(criteria.Id);

            //TODO: Convert domain to models
            var result = new Menu()
            {
                Id = Guid.NewGuid(),
                RestaurantId = Guid.NewGuid(),
                Name = "Lunch Menu (dummy)",
                Description = "This is a dummy item",
                Enabled = true
            };

            return Task.FromResult(result);
        }
    }
}
